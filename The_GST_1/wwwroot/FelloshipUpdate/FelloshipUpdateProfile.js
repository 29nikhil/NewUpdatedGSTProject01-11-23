//_________________________________________________________________________________________________________________________________________________________________
////Dp Change

//function loadFile(event) {

//    console.log("Change Picture")
//    var image = document.getElementById("output");
//    var oldImageInput = document.getElementById("oldImage");
//    image.src = URL.createObjectURL(event.target.files[0]);
//    var oldImageInput = document.getElementById("oldImage");
//    // Pass the file input to uploadImage function
//    uploadImage(event.target.files[0]);
//}

//function uploadImage(file) {
//    var formData = new FormData();
//    formData.append("image", file);


//    var userId = document.getElementById("userId");
//    var userIdValue = userId.value;
//    formData.append("userId", userIdValue);
//    var oldImageInput = document.getElementById("oldImage");
//    var oldImageValue = oldImageInput.value;
//    formData.append("oldimage", oldImageValue);

//    fetch("/UserDetails/ProfilePicChange", {
//        method: "POST",
//        body: formData,
//    })
//        .then(response => response.json())
//        .then(result => {
//            console.log("Server Response:", result);
//            // Handle the server response
//        })
//        .catch(error => {
//            console.error("Error uploading images:", error);
//        });
//}






document.getElementById('registerButton').addEventListener('click', function (event) {
    // Get form fields
    var FirstName = document.getElementById('exampleFirstName').value;
    var MiddleName = document.getElementById('exampleMiddleName').value;
    var LastName = document.getElementById('exampleLastName').value;
    var PhoneNo = document.getElementById('examplePhoneName').value;
    var Email = document.getElementById('emailInput').value;
    var City = document.getElementById('exampleCityName').value;
    var Country = document.getElementById('exampleCountry').value;
    var Address = document.getElementById('exampleAddress').value;

    // Get the submit button
    var errorMessage = '';
    console.log("Submit Button Hit");

    if (!PhoneNo) {
        errorMessage += 'Phone Number is required.\n';
    } else if (!validatePhone(PhoneNo)) {
        errorMessage += 'Please enter a valid Phone Number.\n';
    }

    if (!Email) {
        errorMessage += 'Email is required.\n';
    } else if (!validateEmail(Email)) {
        errorMessage += 'Please enter a valid Email.\n';
    }

    // Check for null or empty values
    if (!FirstName || !MiddleName || !LastName || !PhoneNo || !Email || !City || !Country || !Address) {
        errorMessage += 'All fields are required. Please enter all fields.\n';
    }

    if (errorMessage !== '') {
        Toastify({
            text: "All fields are required. Please enter all fields.",
            duration: 4000,
            gravity: 'up',
            position: 'right',
            backgroundColor: 'red',
        }).showToast();
        // Prevent form submission if there are errors
    } else {
        // Proceed to the next form or other actions
    }
});









function checkEmail() {
    var email = document.getElementById('emailInput').value.trim();
    var submitButton = document.getElementById("registerButton");
    if (email) {



        $.ajax({
            type: 'POST',
            url: '/UserDetails/CheckEmail',
            data: { email: email },
            success: function (result) {
                console.log(result);
                if (result) {
                    $('#resultMessage').text('Email is available.').css('color', 'green');
                    submitButton.disabled = false;

                } else {
                    $('#resultMessage').text('This email is already taken. Please enter a new email.').css('color', 'red');
                    submitButton.disabled = true;
                    event.preventDefault(submitButton)
                }
            },
            error: function (error) {
                console.error('Error:', error);
            }
        });
    }
    else {

        $('#resultMessage').text('').css('color', 'red');

    }
}
//function restrictLength(input, maxLength) {
//    const value = input.value.toString(); // Convert the numeric value to a string
//    if (value.length > maxLength) {
//        input.value = value.slice(0, maxLength); // Trim the value to the maximum length
//    }
//}





//----------------------------------------------------------------------------------------------------------------------------
//Email Validation
function validateEmail(email) {
    console.log("Email Validated");
    var emailRegex = /^[^\s@@]+@@[^\s@@]+\.[^\s@@]+$/;
    return emailRegex.test(email);
}
//Phone No Validation
function validatePhone(phone) {
    console.log("Phone No Validated");
    // Validate if the input is a valid number and doesn't start with zero (optional: and has 10 digits)
    return !isNaN(phone) && phone.length === 10 && phone.charAt(0) !== '0';
}

//_________________________________________________________________________________________________________________________________________________________________



function restrictLengthUpdateProfile(input, maxLength) {
    var registerButton = document.getElementById('registerSubmit');
    const value = input.value.toString(); // Convert the numeric value to a string

    if (value.length > maxLength) {
        console.log("Value trimmed to the maximum length Profile update");

        // Trim the value to the maximum length
        input.value = value.slice(0, maxLength);
        registerButton.disabled = true; // Disable the button when the length exceeds the maximum
    } else {
        registerButton.disabled = false; // Enable the button when the length is within the maximum
    }
}