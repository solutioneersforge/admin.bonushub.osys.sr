// Function to compare two images pixel by pixel
async function compareImagesPixelByPixel(image1Url, image2File) {
    return new Promise((resolve, reject) => {
        const canvas = document.createElement('canvas');
        const ctx = canvas.getContext('2d');

        const img1 = new Image();
        const img2 = new Image();

        img1.crossOrigin = "Anonymous"; // Allow loading images from external URLs
        img1.src = image1Url;

        img1.onload = () => {
            canvas.width = img1.width;
            canvas.height = img1.height;
            ctx.drawImage(img1, 0, 0);
            const imageData1 = ctx.getImageData(0, 0, img1.width, img1.height).data;

            const reader = new FileReader();
            reader.onload = (e) => {
                img2.src = e.target.result;

                img2.onload = () => {
                    if (img2.width !== img1.width || img2.height !== img1.height) {
                        resolve(false); // Different dimensions
                    }

                    ctx.drawImage(img2, 0, 0);
                    const imageData2 = ctx.getImageData(0, 0, img2.width, img2.height).data;

                    // Compare pixel data
                    for (let i = 0; i < imageData1.length; i++) {
                        if (imageData1[i] !== imageData2[i]) {
                            resolve(false); // Different pixels
                        }
                    }

                    resolve(true); // Images are identical
                };
            };

            reader.readAsDataURL(image2File);
        };

        img1.onerror = () => {
            reject("Failed to load the Blob image from the URL.");
        };
    });
}

// Export the function to be used in Blazor
window.imageComparer = {
    compareImagesPixelByPixel
};