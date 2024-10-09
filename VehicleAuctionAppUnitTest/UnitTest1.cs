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
            if (propertyType == typeof(string))
            {
                Assert.IsTrue(jsonProperty.ValueKind == JsonValueKind.String, $"Property '{property.Name}' is not a string.");
            }
            else if (propertyType == typeof(int))
            {
                Assert.IsTrue(jsonProperty.ValueKind == JsonValueKind.Number, $"Property '{property.Name}' is not a number.");
            }
            else if (propertyType == typeof(decimal))
            {
                Assert.IsTrue(jsonProperty.ValueKind == JsonValueKind.Number, $"Property '{property.Name}' is not a number.");
            }
            else if (propertyType == typeof(bool))
            {
                Assert.IsTrue(jsonProperty.ValueKind == JsonValueKind.True || jsonProperty.ValueKind == JsonValueKind.False, $"Property '{property.Name}' is not a boolean.");
            }
            else if (propertyType == typeof(DateTime))
            {
                Assert.IsTrue(jsonProperty.ValueKind == JsonValueKind.String, $"Property '{property.Name}' is not a string.");
                Assert.IsTrue(DateTime.TryParse(jsonProperty.GetString(), out _), $"Property '{property.Name}' is not a valid DateTime.");
            }
            else if (property.PropertyType.IsGenericType && typeof(System.Collections.IEnumerable).IsAssignableFrom(property.PropertyType))
            {

                Assert.IsTrue(jsonProperty.ValueKind == JsonValueKind.Array, $"Property {property.Name} should be an array.");

                Type itemType = property.PropertyType.GetGenericArguments()[0];
                foreach (var item in jsonProperty.EnumerateArray())
                {
                    if (itemType == typeof(string))
                    {
                        Assert.IsTrue(item.ValueKind == JsonValueKind.String, $"Array item in {property.Name} should be a string.");
                    }
                    else if (itemType == typeof(int) || itemType == typeof(int?))
                    {
                        Assert.IsTrue(item.ValueKind == JsonValueKind.Number, $"Array item in {property.Name} should be a number.");
                    }
                }
            }
            else if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
            {
                ValidateModelAgainstJson(propertyType, jsonProperty);
            }

            else
            {
                Assert.Fail($"Unsupported property type: {propertyType.Name}");
            }
        }
    }
}