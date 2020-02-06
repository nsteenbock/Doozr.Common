using Doozr.Common.Isolation.Io;
using Xunit;

namespace Doozr.Common.Isolation.Tests.Io
{
	public class FilesystemTests
	{
		[Fact]
		public void GetFilesRecursive_AllTextFilesInDummyFolderStructure()
		{
			var sut = new Filesystem();

			var files = sut.GetFilesRecursive(@"Io\DummyFolderStructure", "*.txt");

			Assert.Equal(2, files.Length);
			Assert.Equal(@"Io\DummyFolderStructure\TextFile1.txt", files[0]);
			Assert.Equal(@"Io\DummyFolderStructure\Subfolder\TextFileInSubfolder1.txt", files[1]);
		}

		[Fact]
		public void GetFilesRecursive_AllTextFilesInSubfolderOfDummyFolderStructure()
		{
			var sut = new Filesystem();

			var files = sut.GetFilesRecursive(@"Io\DummyFolderStructure\Subfolder", "*.txt");

			Assert.Single(files);
			Assert.Equal(@"Io\DummyFolderStructure\Subfolder\TextFileInSubfolder1.txt", files[0]);
		}

		[Fact]
		public void GetFilesRecursive_AllJpegFilesInDummyFolderStructure()
		{
			var sut = new Filesystem();

			var files = sut.GetFilesRecursive(@"Io\DummyFolderStructure", "*.jpeg");

			Assert.Empty(files);
		}

		[Fact]
		public void GetFiles_AllTextFilesInSubfolderOfDummyFolderStructure()
		{
			var sut = new Filesystem();

			var files = sut.GetFiles(@"Io\DummyFolderStructure\Subfolder", "*.txt");
			
			Assert.Single(files);
			Assert.Equal(@"Io\DummyFolderStructure\Subfolder\TextFileInSubfolder1.txt", files[0]);
		}

		[Fact]
		public void GetFiles_AllTextFilesInDummyFolderStructure()
		{
			var sut = new Filesystem();

			var files = sut.GetFiles(@"Io\DummyFolderStructure", "*.txt");

			Assert.Single(files);
			Assert.Equal(@"Io\DummyFolderStructure\TextFile1.txt", files[0]);
		}

		[Fact]
		public void GetFiles_AllJpegFilesInDummyFolderStructure()
		{
			var sut = new Filesystem();

			var files = sut.GetFiles(@"Io\DummyFolderStructure", "*.jpeg");

			Assert.Empty(files);
		}

		[Fact]
		public void ReadAllText_WithExistingTextFile()
		{
			var sut = new Filesystem();

			var content = sut.ReadAllText(@"Io\DummyFolderStructure\TextFile1.txt");

			Assert.Equal("This is content.", content);
		}
	}
}
