// Function to compute the MD5 hash of a file
async function computeMD5Hash(file) {
    return new Promise((resolve, reject) => {
        const chunkSize = 2097152; // Read in chunks of 2MB
        const spark = new SparkMD5.ArrayBuffer();
        const fileReader = new FileReader();

        let cursor = 0; // Current position in the file

        fileReader.onerror = () => {
            reject("Error reading file.");
        };

        fileReader.onload = (e) => {
            spark.append(e.target.result); // Append array buffer
            cursor += chunkSize;

            if (cursor < file.size) {
                // Read the next chunk
                readNextChunk();
            } else {
                // Finished reading the file
                resolve(spark.end()); // Compute the final hash
            }
        };

        function readNextChunk() {
            const chunk = file.slice(cursor, cursor + chunkSize);
            fileReader.readAsArrayBuffer(chunk);
        }

        // Start reading the first chunk
        readNextChunk();
    });
}

// Function to compute the MD5 hash of an image from a URL
async function computeMD5HashFromUrl(imageUrl) {
    return new Promise((resolve, reject) => {
        const img = new Image();
        img.crossOrigin = "Anonymous"; // Allow loading images from external URLs
        img.src = imageUrl;

        img.onload = () => {
            const canvas = document.createElement('canvas');
            canvas.width = img.width;
            canvas.height = img.height;

            const ctx = canvas.getContext('2d');
            ctx.drawImage(img, 0, 0);

            canvas.toBlob(async (blob) => {
                const hash = await computeMD5Hash(blob);
                resolve(hash);
            }, 'image/jpeg');
        };

        img.onerror = () => {
            reject("Failed to load the image from the URL.");
        };
    });
}

// Function to compute the MD5 hash of an image from a base64 string
async function computeMD5HashFromBase64(base64Image) {
    return new Promise((resolve, reject) => {
        const img = new Image();
        img.src = base64Image;

        img.onload = () => {
            const canvas = document.createElement('canvas');
            canvas.width = img.width;
            canvas.height = img.height;

            const ctx = canvas.getContext('2d');
            ctx.drawImage(img, 0, 0);

            canvas.toBlob(async (blob) => {
                const hash = await computeMD5Hash(blob);
                resolve(hash);
            }, 'image/jpeg');
        };

        img.onerror = () => {
            reject("Failed to load the base64 image.");
        };
    });
}

// Export the functions to be used in Blazor
window.fileHasher = {
    computeMD5HashFromUrl,
    computeMD5HashFromBase64
};