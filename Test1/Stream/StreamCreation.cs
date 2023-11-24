namespace Test1.Stream
{
	public class StreamCreation
	{
		public async Task CopyFolderAsync(string sourceFolder, string destFolder, string name, string imageURL)
		{
			System.Threading.Thread.Sleep(5000);
			try
			{
				if (!Directory.Exists(destFolder))
				{
					Directory.CreateDirectory(destFolder);
				}

				foreach (string file in Directory.GetFiles(sourceFolder))
				{
					string destFile = Path.Combine(destFolder, Path.GetFileName(file));
					File.Copy(file, destFile, false);

					if(file.Contains("index.html"))
					{
						string fileContent = File.ReadAllText(file);
						fileContent = fileContent.Replace("change1startshere", name);
						fileContent = fileContent.Replace("change2startshere", imageURL);
						File.WriteAllText(destFile, fileContent);
					}
				}

				foreach (string folder in Directory.GetDirectories(sourceFolder))
				{
					string destSubFolder = Path.Combine(destFolder, Path.GetFileName(folder));
					await CopyFolderAsync(folder, destSubFolder, name, imageURL);
				}
			}
			catch (Exception ex)
			{
				// Log or handle the exception here
				Console.WriteLine($"An error occurred: {ex.Message}");
			}
		}

		public async Task NewStreamCreation(string name, string imageURL)
		{
			string wwwRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", name);
			string templatesFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Template");

			await CopyFolderAsync(templatesFolderPath, wwwRootPath, name, imageURL);
		}

		public async Task StreamUpdate(string oldname, string oldImageURL, string newName, string newImageURL)
		{
			System.Threading.Thread.Sleep(5000);
			if (oldname == newName)
			{
				string htmlFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", oldname, "index.html");
				string fileContent = File.ReadAllText(htmlFilePath);
				fileContent = fileContent.Replace(oldImageURL, newImageURL);
				File.WriteAllText(htmlFilePath, fileContent);
			}
			else
			{
				string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", oldname);
				Directory.Delete(folderPath, true);
				await NewStreamCreation(newName, newImageURL);
			}
		}
	}
}