# Test Images for Spectra API

## ✅ Cross-Platform Image Processing

The Spectra Image Management API now uses **SixLabors.ImageSharp** for cross-platform image processing, replacing the Windows-only System.Drawing.Common library. This ensures the application works correctly on Windows, macOS, and Linux.

## Required Test Files

To test the Image Management API, you need to provide actual image files:

1. **test-image.png** - A PNG image file for testing uploads (✅ Created successfully)
2. **test-image2.jpg** - Another image file for testing multiple uploads

## How to Create Test Images

### Option 1: Use any existing images
- Copy any JPEG, PNG, GIF, or WebP image files to the project root
- Rename them to `test-image.jpg` and `test-image2.jpg`

### Option 2: Create simple test images using Python (Recommended)
```bash
# Create test images using Python PIL (works on all platforms)
python3 -c "
from PIL import Image
img = Image.new('RGB', (300, 200), color='red')
img.save('test-image.png')
img2 = Image.new('RGB', (400, 300), color='blue')
img2.save('test-image2.jpg')
print('Test images created successfully')
"
```

### Option 3: Create simple test images (macOS/Linux with ImageMagick)
```bash
# Create a simple colored rectangle using ImageMagick (if installed)
convert -size 800x600 xc:blue test-image.jpg
convert -size 600x800 xc:red test-image2.jpg
```

### Option 4: Download from the internet
```bash
# Download sample images (requires internet connection)
curl -L "https://via.placeholder.com/800x600/0000FF/FFFFFF.jpg?text=Test+Image+1" -o test-image.jpg
curl -L "https://via.placeholder.com/600x800/FF0000/FFFFFF.jpg?text=Test+Image+2" -o test-image2.jpg
```

### Option 5: Use screenshots
- Take screenshots and save them as `test-image.png` and `test-image2.jpg`

## File Requirements

- **Supported formats**: JPEG, PNG, GIF, WebP
- **Maximum file size**: 5MB
- **Recommended size**: 800x600 or similar for testing

## ✅ ImageSharp Implementation Benefits

The new SixLabors.ImageSharp implementation provides:

- **Cross-platform compatibility**: Works on Windows, macOS, and Linux
- **Enhanced validation**: Better image format detection and validation
- **Improved performance**: More efficient image processing
- **No platform warnings**: Eliminates Windows-only dependency warnings
- **Better error handling**: More detailed validation error messages

## Testing Notes

- The test file `test-images.http` references these image files
- Make sure the files exist in the project root directory before running tests
- The API will validate file type, size, and format using ImageSharp
- Enhanced validation ensures only valid image files are accepted
- Images will be stored in `Spectra.ApiService/wwwroot/uploads/images/`
- Image dimensions are automatically detected and stored
