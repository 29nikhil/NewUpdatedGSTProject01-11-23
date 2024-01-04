//Dp Change

function loadFile(event) {

    console.log("Change Picture")
    var image = document.getElementById("output");
    var oldImageInput = document.getElementById("oldImage");
    image.src = URL.createObjectURL(event.target.files[0]);
    var oldImageInput = document.getElementById("oldImage");
    // Pass the file input to uploadImage function
    uploadImage(event.target.files[0]);
}

function uploadImage(file) {
    var formData = new FormData();
    formData.append("image", file);


    var userId = document.getElementById("userId");
    var userIdValue = userId.value;
    formData.append("userId", userIdValue);
    var oldImageInput = document.getElementById("oldImage");
    var oldImageValue = oldImageInput.value;
    formData.append("oldimage", oldImageValue);

    fetch("/UserDetails/ProfilePicChange", {
        method: "POST",
        body: formData,
    })
        .then(response => response.json())
        .then(result => {
            console.log("Server Response:", result);
            // Handle the server response
        })
        .catch(error => {
            console.error("Error uploading images:", error);
        });
}

