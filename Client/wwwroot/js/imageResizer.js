function resizeImage(imageElement, minWidth, minHeight, maxWidth, maxHeight) {
    return new Promise((resolve) => {
        const canvas = document.createElement('canvas');
        const ctx = canvas.getContext('2d');

        let width = imageElement.width;
        let height = imageElement.height;

        // Calculate new dimensions while maintaining aspect ratio
        const minRatio = Math.max(minWidth / width, minHeight / height);
        const maxRatio = Math.min(maxWidth / width, maxHeight / height);

        // Ensure the image fits within the min and max dimensions
        const ratio = Math.min(maxRatio, Math.max(minRatio, 1));

        width *= ratio;
        height *= ratio;

        // Set canvas dimensions
        canvas.width = width;
        canvas.height = height;

        // Draw the resized image on the canvas
        ctx.drawImage(imageElement, 0, 0, width, height);

        // Convert the canvas to a data URL (base64)
        const resizedImageDataUrl = canvas.toDataURL('image/jpeg', 0.8);
        resolve(resizedImageDataUrl);
    });
}

// Function to load an image from a base64 string or byte array and resize it
window.resizeImageFromBase64 = async (imageData, minWidth, minHeight, maxWidth, maxHeight) => {
    return new Promise((resolve) => {
        const img = new Image();
        img.onload = () => {
            resizeImage(img, minWidth, minHeight, maxWidth, maxHeight).then(resizedImageDataUrl => {
                resolve(resizedImageDataUrl);
            });
        };
        img.src = imageData;
    });
};