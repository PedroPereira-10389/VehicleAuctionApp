using System.Reflection;
using System.Text.Json.Serialization;
using System.Text.Json;
using VehicleAuctionApp.Services;
using VehicleAuctionApp.Models;

namespace VehicleAuctionAppUnitTest
{
    public class Tests
    {
        private LoadingService _loadingService;
        private string? _jsonContent;

        [SetUp]
        public void Setup()
        {
            _loadingService = new LoadingService();
        }

        [Test, Order(1)]
        public void LoadingServiceResults()
        {
            var jsonFilePath = Path.Combine(TestContext.CurrentContext.TestDirectory, "Resources", "Raw", "vehicles_dataset.json");
            Assert.IsTrue(File.Exists(jsonFilePath), $"JSON file not found: {jsonFilePath}");
            _jsonContent = File.ReadAllText(jsonFilePath);
            Assert.IsNotEmpty(_jsonContent, "JSON content is empty");
        }

        [Test, Order(2)]
        public void CheckJsonFileStructure()
        {
            Assert.IsNotNull(_jsonContent, "_jsonContent is null");
            using var document = JsonDocument.Parse(_jsonContent);
            JsonElement root = document.RootElement;
            Assert.IsTrue(root.ValueKind == JsonValueKind.Array, "The root element is not an array");

            foreach (JsonElement vehicleElement in root.EnumerateArray())
            {
                ValidateModelAgainstJson(typeof(Vehicle), vehicleElement);
            }
        }

        private void ValidateModelAgainstJson(Type modelType, JsonElement jsonElement)
        {
            PropertyInfo[] properties = modelType.GetProperties();


            foreach (var property in properties)
            {
                if (!property.CanWrite) //Ignore calculated properties
                {
                    continue;
                }

                var jsonPropertyName = property.GetCustomAttribute<JsonPropertyNameAttribute>()?.Name ?? property.Name;

                bool isOptional = property.PropertyType == typeof(string) || Nullable.GetUnderlyingType(property.PropertyType) != null;

                if (isOptional)
                {
                    if (jsonElement.TryGetProperty(jsonPropertyName, out JsonElement jsonProperty))
                    {
                        ValidatePropertyType(property, jsonProperty);
                    }
                    else
                    {
                        TestContext.WriteLine($"Optional property '{jsonPropertyName}' is not present in the JSON.");
                    }
                }
                else
                {
                    Assert.IsTrue(jsonElement.TryGetProperty(jsonPropertyName, out JsonElement jsonProperty),
                   $"JSON is missing required property: {jsonPropertyName}");
                    ValidatePropertyType(property, jsonProperty);
                }
            }
        }

        private void ValidatePropertyType(PropertyInfo property, JsonElement jsonProperty)
        {
            Type propertyType = property.PropertyType;

            switch (propertyType)
            {
                case Type t when t == typeof(string):
                    Assert.IsTrue(jsonProperty.ValueKind == JsonValueKind.String, $"Property '{property.Name}' is not a string.");
                    break;

                case Type t when t == typeof(int):
                    Assert.IsTrue(jsonProperty.ValueKind == JsonValueKind.Number, $"Property '{property.Name}' is not a number.");
                    break;

                case Type t when t == typeof(decimal):
                    Assert.IsTrue(jsonProperty.ValueKind == JsonValueKind.Number, $"Property '{property.Name}' is not a number.");
                    break;

                case Type t when t == typeof(bool):
                    Assert.IsTrue(jsonProperty.ValueKind == JsonValueKind.True || jsonProperty.ValueKind == JsonValueKind.False, $"Property '{property.Name}' is not a boolean.");
                    break;

                case Type t when t == typeof(DateTime):
                    Assert.IsTrue(jsonProperty.ValueKind == JsonValueKind.String, $"Property '{property.Name}' is not a string.");
                    Assert.IsTrue(DateTime.TryParse(jsonProperty.GetString(), out _), $"Property '{property.Name}' is not a valid DateTime.");
                    break;

                case Type t when t.IsGenericType && typeof(System.Collections.IEnumerable).IsAssignableFrom(t):
                    Assert.IsTrue(jsonProperty.ValueKind == JsonValueKind.Array, $"Property {property.Name} should be an array.");

                    Type itemType = t.GetGenericArguments()[0];
                    foreach (var item in jsonProperty.EnumerateArray())
                    {
                        switch (itemType)
                        {
                            case Type it when it == typeof(string):
                                Assert.IsTrue(item.ValueKind == JsonValueKind.String, $"Array item in {property.Name} should be a string.");
                                break;

                            case Type it when it == typeof(int) || it == typeof(int?):
                                Assert.IsTrue(item.ValueKind == JsonValueKind.Number, $"Array item in {property.Name} should be a number.");
                                break;

                            default:
                                Assert.Fail($"Unsupported array item type: {itemType.Name}");
                                break;
                        }
                    }
                    break;

                case Type t when t.IsClass && t != typeof(string):
                    ValidateModelAgainstJson(t, jsonProperty);
                    break;

                default:
                    Assert.Fail($"Unsupported property type: {propertyType.Name}");
                    break;
            }
        }
    }

 }