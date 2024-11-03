using iOSClub.Data.DataModels;
using Newtonsoft.Json;

var file = await File.ReadAllTextAsync(@"C:\Users\李嘉俊\Downloads\output.json");

var json = JsonConvert.DeserializeObject<List<StudentModel>>(file);
if (json == null) return;
foreach (var model in json)
{
    
}