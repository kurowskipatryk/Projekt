using Meziantou.Framework;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using MudBlazor;
using MudBlazor.Components.Chart;
using Projekt.Models;
using System;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.IO.Pipes;
using System.Net;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Xml.Linq;
using static MudBlazor.CategoryTypes;
using static MudBlazor.Colors;
using static MudBlazor.Icons;
using static System.Net.Mime.MediaTypeNames;
using static System.Net.WebRequestMethods;
using File = System.IO.File;
using Image = System.Drawing.Image;

namespace Projekt.Pages
{
    public partial class Watermark
    {
        [Inject] ISnackbar Snackbar { get; set; }
        IReadOnlyList<IBrowserFile> files = new List<IBrowserFile>();
        private long maxFileSize = 3 * 1024 * 1024; //max 3Mb 

        string pathFolder = "wwwroot/Photos/";
        List<FileUploadProgress> uploadedFiles = new();
        int numberOfEmptyImages = 6;
        public List<FileBase> FileList { get; set; } = new List<FileBase>();

        string watermarkText = "WATERMARK";
        int watermarkOption = 1;
        string pathEmpty = "wwwroot/Img/empty/empty.jpg";

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {

                using (MemoryStream m = new MemoryStream())
                {
                    byte[] imageBytes = System.IO.File.ReadAllBytes(pathEmpty);
                    //image.Save(m, image.RawFormat);
                    //byte[] imageBytes = m.ToArray();

                    // Convert byte[] to Base64 String
                    base64String = Convert.ToBase64String(imageBytes);
                }
                //using (Image image = Image.FromFile(pathEmpty))
                //{
                //    using (MemoryStream m = new MemoryStream())
                //    {
                //        image.Save(m, image.RawFormat);
                //        byte[] imageBytes = m.ToArray();

                //        // Convert byte[] to Base64 String
                //        base64String = Convert.ToBase64String(imageBytes);
                //    }
                //}

                if (watermarkOption == 2)
                {
                    await ChangeSize(30);
                }
                else
                {
                    await ChangeSize(18);

                }
                if (AnchorOrigin == Origin.CenterLeft)
                {
                    var dotNetObjRef = DotNetObjectReference.Create(this);
                    await JSRuntime.InvokeVoidAsync("dragElement", dotNetObjRef);
                }
                await ChangeOption(1);
                await ChangeOrigin(Origin.BottomRight);
                await ChangeColor(System.Drawing.Color.White);
                StateHasChanged();
            }
        }

        double leftPercent;
        double topPercent;
        double leftW;
        double topH;
        [JSInvokable]
        public async Task GetPercentFromJS(double left, double top, double width, double height)
        {
            leftPercent = left;
            topPercent = top;
            leftW = width;
            topH = height;
        }

        bool selectedFile;
        public int watermakrSize = 18;
        private async Task ChangeSize(int value)
        {
            watermakrSize = value;
            if (AnchorOrigin == Origin.CenterLeft)
            {
                if (watermarkOption == 1)
                {
                    await JSRuntime.InvokeVoidAsync("applyStyle", new { element = "mydivheader", attrib = "fontSize", value = value / 3 });
                }
                else
                {
                    await JSRuntime.InvokeVoidAsync("applyStyle2", new { element = "mydivheader", attrib = "fontSize", value = value });
                }
            }
            else
            {
                await ChangeEmpty();

            }

        }
        private async Task ChangeOption(int option)
        {
            selectedFile = false;
            watermarkOption = option;
            await ChangeSize(30);
            await ChangeEmpty();
        }
        string pathAdded;
        string base64StringDrag;
        private async Task UploadFiles2(IBrowserFile file)
        {
            pathAdded = Path.Combine($"wwwroot/PhotosAdd/", file.Name);
            selectedFile = true;
            await using FileStream fs = new FileStream(pathAdded, FileMode.Create);
            await file.OpenReadStream(maxFileSize).CopyToAsync(fs);
            var bytes = new byte[file.Size];
            fs.Position = 0;
            await fs.ReadAsync(bytes);
            fs.Close();

            using (MemoryStream m = new MemoryStream())
            {
                byte[] imageBytes = System.IO.File.ReadAllBytes(pathAdded);
            
                // Convert byte[] to Base64 String
                base64String = Convert.ToBase64String(imageBytes);
            }


            //using (Image image = Image.FromFile(pathAdded))
            //{
            //    using (MemoryStream m = new MemoryStream())
            //    {
            //        image.Save(m, image.RawFormat);
            //        byte[] imageBytes = m.ToArray();

            //        base64StringDrag = Convert.ToBase64String(imageBytes);
            //    }
            //}
            await ChangeSize(watermakrSize);
        }

        bool self;
        private async Task ChangeOrigin(Origin origin)
        {
            AnchorOrigin = origin;
            if (AnchorOrigin == Origin.CenterLeft)
            {
                await ChangeToEmpty();
                self = true;
                StateHasChanged();
                await InvokeAsync(StateHasChanged);
                await Task.Delay(1000);
                await ChangeSize(30);
                var dotNetObjRef = DotNetObjectReference.Create(this);
                await JSRuntime.InvokeVoidAsync("dragElement", dotNetObjRef);
            }
            else
            {
                self = false;
                await ChangeEmpty();
            }
        }



        private async Task ChangeColor(System.Drawing.Color clr)
        {
            color1 = clr;
            if (color1 == System.Drawing.Color.White)
            {
                color2 = System.Drawing.Color.DarkGray;
            }
            else
            {
                color2 = System.Drawing.Color.Black;
            }
            await ChangeEmpty();
        }
        string base64String;
        private async Task ChangeEmpty()
        {
            if ((selectedFile && watermarkOption == 2) || (watermarkText.Length != 0 && watermarkOption == 1))
            {

                byte[] fileContents = File.ReadAllBytes(pathEmpty);
                using (MemoryStream memoryStream = new MemoryStream(fileContents))
                {
                    var x = await AddTextWatermark(memoryStream, watermarkText, color1, color2, watermakrSize);

                    byte[] imageBytes = x.ToArray();
                    base64String = Convert.ToBase64String(imageBytes);


                    using (TextReader textReader = new StreamReader(memoryStream))
                    {
                        string line;
                        while ((line = textReader.ReadLine()) != null)
                            Console.WriteLine(line);
                    }
                }
            }
            StateHasChanged();
        }

        private async Task ChangeToEmpty()
        {
            using (Image image = Image.FromFile(pathEmpty))
            {
                using (MemoryStream m = new MemoryStream())
                {
                    image.Save(m, image.RawFormat);
                    byte[] imageBytes = m.ToArray();

                    // Convert byte[] to Base64 String
                    base64String = Convert.ToBase64String(imageBytes);
                }
            }
        }

        private async ValueTask LoadFiles(InputFileChangeEventArgs e)
        {
            files = e.GetMultipleFiles(maximumFileCount: numberOfEmptyImages);
            //FileList.Clear();
            var startIndex = uploadedFiles.Count;

            // Add all files to the UI
            foreach (var file in files)
            {
                var progress = new FileUploadProgress(file.Name, file.Size);
                uploadedFiles.Add(progress);
            }

            // We don't want to refresh the UI too frequently,
            // So, we use a timer to update the UI every few hundred milliseconds
            await using var timer = new Timer(_ => InvokeAsync(() => StateHasChanged()));
            timer.Change(TimeSpan.FromMilliseconds(1000), TimeSpan.FromMilliseconds(500));

            // Upload files
            byte[] buffer = System.Buffers.ArrayPool<byte>.Shared.Rent(4096);
            try
            {
                foreach (var file in files)
                {
                    using var stream = file.OpenReadStream(maxAllowedSize: maxFileSize);
                    while (await stream.ReadAsync(buffer) is int read && read > 0)
                    {
                        uploadedFiles[startIndex].UploadedBytes += read;

                        // TODO Do something with the file chunk, such as save it
                        // to a database or a local file system
                        var readData = buffer.AsMemory().Slice(0, read);
                    }

                    startIndex++;
                }
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Error: {ex.Message}", Severity.Warning);

                foreach (var item in FileList)
                {
                    await DeleteFile(item.Name);
                    FileList.RemoveAll(x => x.Name == item.Name);

                }
                uploadedFiles = new List<FileUploadProgress>();
                files = new List<IBrowserFile>();
                numberOfEmptyImages = 6;
                StateHasChanged();
                throw;
            }
            finally
            {
                System.Buffers.ArrayPool<byte>.Shared.Return(buffer);
                foreach (var file in files)
                {
                    var fileName = $"{Guid.NewGuid()}{file.Name}";
                    var path = Path.Combine($"wwwroot/Photos/", fileName);

                    var image = new FileBase
                    {
                        Name = fileName,
                        Path = path,
                        Old_Name = file.Name,
                    };
                    FileList.Add(image);

                    await using FileStream fs = new FileStream(path, FileMode.Create);
                    await file.OpenReadStream(maxFileSize).CopyToAsync(fs);
                    var bytes = new byte[file.Size];
                    fs.Position = 0;
                    await fs.ReadAsync(bytes);
                    fs.Close();
                    byte[] fileContents = File.ReadAllBytes(path);
                    using (MemoryStream memoryStream = new MemoryStream(fileContents))
                    {
                        var x = await AddTextWatermark(memoryStream, watermarkText, color1, color2, watermakrSize);
                        x.Seek(0, SeekOrigin.Begin);
                        using (var fs1 = new FileStream(path, FileMode.OpenOrCreate))
                        {
                            x.CopyTo(fs1);
                        }
                        using (TextReader textReader = new StreamReader(memoryStream))
                        {
                            string line;
                            while ((line = textReader.ReadLine()) != null)
                                Console.WriteLine(line);
                        }
                    }
                    numberOfEmptyImages--;
                }
                fileExists = true;
                StateHasChanged();
            }
            StateHasChanged();
        }
        private async Task DeleteFile(string filename)
        {
            try
            {
                // Check if file exists with its full path    
                if (File.Exists(Path.Combine($"{pathFolder}{Path.DirectorySeparatorChar}", filename)))
                {
                    // If file found, delete it    
                    File.Delete(Path.Combine($"{pathFolder}{Path.DirectorySeparatorChar}", filename));
                    Console.WriteLine("File deleted.");
                }
                else Console.WriteLine("File not found");
            }
            catch (IOException ioExp)
            {
                Console.WriteLine(ioExp.Message);
            }
        }
        bool fileExists;
        int selectedPosition = 1;

        public Origin TransformOrigin { get; set; } = Origin.TopCenter;
        public Origin AnchorOrigin { get; set; } = Origin.BottomRight;


        public string GetLocation()
        {
            string align = "";
            string justify = "";
            string[] anch = AnchorOrigin.ToDescriptionString().Split("-");

            if (anch[0] == "top" && anch[1] == "left")
            {
                align = "align-start";
                justify = "justify-start";
            }
            else if (anch[0] == "top" && anch[1] == "right")
            {
                align = "align-start";
                justify = "justify-end";
            }
            else if (anch[0] == "bottom" && anch[1] == "left")
            {
                align = "align-end";
                justify = "justify-start";
            }
            else if (anch[0] == "bottom" && anch[1] == "right")
            {
                align = "align-end";
                justify = "justify-end";
            }


            return $"absolute mud-height-full mud-width-full d-flex {align} {justify}";
        }

        public Image SetImageOpacity(Image image, float opacity)
        {
            try
            {
                //create a Bitmap the size of the image provided  
                Bitmap bmp = new Bitmap(image.Width, image.Height);

                //create a graphics object from the image  
                using (Graphics gfx = Graphics.FromImage(bmp))
                {

                    //create a color matrix object  
                    ColorMatrix matrix = new ColorMatrix();

                    //set the opacity  
                    matrix.Matrix33 = opacity;

                    //create image attributes  
                    ImageAttributes attributes = new ImageAttributes();

                    //set the color(opacity) of the image  
                    attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                    //now draw the image  
                    gfx.DrawImage(image, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, attributes);
                }
                return bmp;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return null;
            }
        }



        public System.Drawing.Color color1 = System.Drawing.Color.White;
        public System.Drawing.Color color2 = System.Drawing.Color.DarkGray;
        public async Task<MemoryStream> AddTextWatermark(MemoryStream imageStream, string watermarkText, System.Drawing.Color color1, System.Drawing.Color color2, int watermakSize)
        {
            using (Bitmap oBitmap = new Bitmap(imageStream, false))
            {
                using (Graphics oGraphics = Graphics.FromImage(oBitmap))
                {
                    if (watermarkOption == 2)
                    {
                        SizeF oSizeF = new SizeF();


                        byte[] fileContents = File.ReadAllBytes(pathAdded);
                        using (MemoryStream memoryStream = new MemoryStream(fileContents))
                        {
                            var img = Image.FromStream(memoryStream);
                            img = SetImageOpacity(img, 0.4f);

                            double dbl = (double)img.Width / (double)img.Height;
                            var perc = Convert.ToDouble(watermakSize) / 100;
                            int widthW = (int)(oBitmap.Width * (Convert.ToDouble(watermakSize) / 100));
                            int heightW = (int)((oBitmap.Height * (Convert.ToDouble(watermakSize) / 100)) / dbl);

                            img = (Image)new Bitmap(img, widthW, heightW);

                            Point oPoint = new Point(10, 10);
                            if (AnchorOrigin == Origin.TopRight)
                            {
                                oPoint = new Point(oBitmap.Width - (img.Width + 10), 10);
                            }
                            else if (AnchorOrigin == Origin.BottomRight)
                            {
                                oPoint = new Point(oBitmap.Width - (img.Width + 10), oBitmap.Height - (img.Height + 10));
                            }
                            else if (AnchorOrigin == Origin.BottomLeft)
                            {
                                oPoint = new Point(10, oBitmap.Height - (img.Height + 10));
                            }
                            else if (AnchorOrigin == Origin.CenterCenter)
                            {
                                oPoint = new Point(oBitmap.Width / 2 - (img.Width / 2), oBitmap.Height / 2 - (img.Height / 2));
                            }
                            else if (AnchorOrigin == Origin.CenterLeft)
                            {
                                var x = (int)((double)oBitmap.Width * leftPercent);
                                var y = (int)((double)oBitmap.Height * topPercent);

                                //oPoint = new Point( x + (int)leftW, y + (int)topH);
                                oPoint = new Point( x - ((int)img.Width /2 ), y - ((int)img.Height /2) );
                            }

                            oGraphics.DrawImage(img, new Rectangle(oPoint.X, oPoint.Y, img.Width, img.Height));

                        }
                    }
                    else
                    {

                        Font oFont = new Font("Arial", watermakSize, FontStyle.Bold, GraphicsUnit.Pixel);
                        if (oBitmap.Width > 4000)
                            oFont = new Font("Arial", watermakSize * 10, FontStyle.Bold, GraphicsUnit.Pixel);
                        else if (oBitmap.Width > 2000)
                            oFont = new Font("Arial", watermakSize * 5, FontStyle.Bold, GraphicsUnit.Pixel);
                        else if (oBitmap.Width > 1000)
                            oFont = new Font("Arial", watermakSize * 3, FontStyle.Bold, GraphicsUnit.Pixel);

                        Brush oBrush = new SolidBrush(System.Drawing.Color.FromArgb(180, 0, 0, 0));
                        if (color1 == System.Drawing.Color.White)
                        {
                            oBrush = new SolidBrush(System.Drawing.Color.FromArgb(80, 0, 0, 0));
                            //oBrush = new HatchBrush(HatchStyle.OutlinedDiamond, System.Drawing.Color.FromArgb(100, 255, 255, 255), System.Drawing.Color.FromArgb(100, 190, 190, 190));

                        }

                        //Font oFont = new Font("Arial", 20, FontStyle.Bold, GraphicsUnit.Pixel);
                        SizeF oSizeF = new SizeF();
                        oSizeF = oGraphics.MeasureString(watermarkText, oFont);

                        Point oPoint = new Point(oBitmap.Width - ((int)oSizeF.Width + 10), oBitmap.Height - ((int)oSizeF.Height + 10));
                        if (AnchorOrigin == Origin.BottomRight)
                        {
                            oPoint = new Point(oBitmap.Width - ((int)oSizeF.Width + 10), oBitmap.Height - ((int)oSizeF.Height + 10));
                        }
                        else if (AnchorOrigin == Origin.BottomLeft)
                        {
                            oPoint = new Point(oBitmap.Width - (oBitmap.Width - 10), oBitmap.Height - ((int)oSizeF.Height + 10));
                        }
                        else if (AnchorOrigin == Origin.TopRight)
                        {
                            oPoint = new Point(oBitmap.Width - ((int)oSizeF.Width + 10), oBitmap.Height - (oBitmap.Height - 10));
                        }
                        else if (AnchorOrigin == Origin.TopLeft)
                        {
                            oPoint = new Point(oBitmap.Width - (oBitmap.Width - 10), oBitmap.Height - (oBitmap.Height - 10));
                        }
                        else if (AnchorOrigin == Origin.CenterCenter)
                        {
                            oPoint = new Point(oBitmap.Width / 2 - (int)(oSizeF.Width / 2), oBitmap.Height / 2 - (int)(oSizeF.Height / 2));
                        }
                        else if (AnchorOrigin == Origin.CenterLeft)
                        {
                            var x = (int)((double)oBitmap.Width * leftPercent);
                            var y = (int)((double)oBitmap.Height * topPercent);

                            oPoint = new Point(x - ((int)oSizeF.Width/2 ), y - ((int)oSizeF.Height/2 ));
                        }
                        oGraphics.DrawString(watermarkText, oFont, oBrush, oPoint);

                    }


                    var resultStream = new MemoryStream();
                    oBitmap.Save(resultStream, ImageFormat.Jpeg);
                    return resultStream;
                }
            }
        }
        [Inject] public IJSRuntime JSRuntime { get; set; }
        private async Task DeleteImage(FileBase fileToDelete)
        {
            bool confirmed = await JSRuntime.InvokeAsync<bool>("confirm", "Are you sure?");
            if (confirmed)
            {
                uploadedFiles.RemoveAll(x => x.FileName == fileToDelete.Old_Name);
                FileList.Remove(fileToDelete);
                numberOfEmptyImages++;
                await DeleteFile(fileToDelete.Name);
            }
        }

        string FormatBytes(long value)
      => ByteSize.FromBytes(value).ToString("fi2", CultureInfo.CurrentCulture);

        record FileUploadProgress(string FileName, long Size)
        {
            public long UploadedBytes { get; set; }
            public double UploadedPercentage => (double)UploadedBytes / (double)Size * 100d;
        }

        private async Task Download()
        {
            foreach (var file in FileList)
            {
                var fileStream = GetFileStream(file.Name);
                using var streamRef = new DotNetStreamReference(stream: fileStream);
                await JSRuntime.InvokeVoidAsync("downloadFileFromStream", file.Name, streamRef);
                await DeleteFile(file.Name);
            }

            numberOfEmptyImages = 6;
            FileList = new List<FileBase>();
            uploadedFiles = new List<FileUploadProgress>();
        }

        private Stream GetFileStream(string fileName)
        {
            return File.OpenRead($"{pathFolder}/{fileName}");
        }
    }
}

