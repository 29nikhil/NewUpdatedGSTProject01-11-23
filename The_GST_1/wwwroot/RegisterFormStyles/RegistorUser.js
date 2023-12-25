﻿


//Password Regular Expression Check Function


var passwordRegex = /^(?=.*[a-zA-Z])(?=.*\d)(?=.*[!@#$%^&*()_+{}\[\]:;<>,.?~\\/-])/;

function isValidPassword(password) {
    return passwordRegex.test(password);
}
//_________________________________________________________________________________________________________________________________________________________________

//Form1 Check Validation After Click On next Button   
//This Method Use 
//1) Account / Registor

document.addEventListener('DOMContentLoaded', function () {
    var nextButton = document.getElementById('nextButton');

    nextButton.addEventListener('click', function () {
        // Perform null check for all fields in the current form
        var FirstName = document.getElementById('exampleFirstName').value;
        var MiddleName = document.getElementById('exampleMiddleName').value;

        var LastName = document.getElementById('exampleLastName').value;
        var PhoneNo = document.getElementById('examplePhoneName').value;
        var Email = document.getElementById('emailInput').value;
        var City = document.getElementById('exampleCityName').value;
        var Country = document.getElementById('exampleCountry').value;
        var Address = document.getElementById('exampleAddress').value;
        var Password = document.getElementById('exampleInputPassword').value;
        var Repassword = document.getElementById('exampleRepeatPassword').value;

        var errorMessage = '';

        if (!FirstName) {
            errorMessage = 'First Name is required.\n';

            var errorContainer = document.getElementById('FirstNameError');
            errorContainer.innerHTML = errorMessage === '' ? '' : errorMessage;
        }
        if (!MiddleName) {
            errorMessage = 'Middle Name is required.\n';
            var errorContainer = document.getElementById('MiddleNameError');
            errorContainer.innerHTML = errorMessage === '' ? '' : errorMessage;
        }
        if (!LastName) {
            errorMessage = 'Last Name is required.\n';

            var errorContainer = document.getElementById('LastNameError');
            errorContainer.innerHTML = errorMessage === '' ? '' : errorMessage;


        }
        if (!PhoneNo) {
            errorMessage = 'Phone Number is required.\n';
            var errorContainer = document.getElementById('PhoneNumberError');
            errorContainer.innerHTML = errorMessage === '' ? '' : errorMessage;
        }
        if (!Email) {
            errorMessage = 'Email is required.\n';
            var errorContainer = document.getElementById('EmailError');
            errorContainer.innerHTML = errorMessage === '' ? '' : errorMessage;
        }
        if (!City) {
            errorMessage = 'City is required.\n';
            var errorContainer = document.getElementById('CityError');
            errorContainer.innerHTML = errorMessage === '' ? '' : errorMessage;
        }
        if (!Country) {
            errorMessage = 'Country is required.\n';
            var errorContainer = document.getElementById('CountryError');
            errorContainer.innerHTML = errorMessage === '' ? '' : errorMessage;
        }
        if (!Address) {
            errorMessage = 'Address is required.\n';
            var errorContainer = document.getElementById('AddressError');
            errorContainer.innerHTML = errorMessage === '' ? '' : errorMessage;
        }
        if (!Password) {
            errorMessage = 'Password is required.\n';
            var errorContainer = document.getElementById('PasswordError');
            errorContainer.innerHTML = errorMessage === '' ? '' : errorMessage;
            
        }
        //if (/[a-zA-Z]/.test(password)) {
        //    debugger
        //    errorMessage += 'Password must contain at least one alphabetical character.\n';
        //}
        if (!Repassword) {
            errorMessage = 'Repeat Password is required.\n';
            var errorContainer = document.getElementById('ConfirmPasswordError');
            errorContainer.innerHTML = errorMessage === '' ? '' : errorMessage;
        }
        if (Password != Repassword) {
            errorMessage = 'The password and confirmation password do not match.\n';
            //var errorContainer = document.getElementById('ConfirmPasswordError');
            //errorContainer.innerHTML = errorMessage === '' ? '' : errorMessage;
        }
        var a = $('#resultMessage');
        console.log(a);
        if (a[0] !== document.getElementById('resultMessage')) {
            errorMessage = 'The password and confirmation password do not match.\n';
        }

       
        if (errorMessage !== '') {
            Toastify({
                text: "All Filed is required. please Enter all Files",
                duration: 4000, // duration in milliseconds
                gravity: 'up', // 'top' or 'bottom'
                position: 'right', // 'left', 'center', or 'right'
                backgroundColor: 'red',
            }).showToast();
        } else {
            // Proceed to the next form
            showNextForm();
        }

    });


});


//_________________________________________________________________________________________________________________________________________________________________



// Form2 Check Validation After Click On Submit  Button
// This is Function Submit Button click on Check Validation and Show Toast
//1) Account / Registor

document.getElementById('registerButton').addEventListener('click', function () {
    /// Get form fields
    var BusinessType = document.getElementById('exampleBusinessType').value;
    var PanNo = document.getElementById('examplePANNo').value;
    var AdharNo = document.getElementById('exampleAdharNo').value;
    var Website = document.getElementById('examplewebsite').value;
    var GstNo = document.getElementById('Gstno').value;
    var AdharFile = document.getElementById('fileAdharInput').value;
    var PanFile = document.getElementById('fileInput').value;
    
    // Get the submit button
    var submitButton = document.getElementById('registerButton');
    
    // Check for null or empty values
    if (!BusinessType || !PanNo || !AdharNo || !Website || !GstNo || !AdharFile || !PanFile) {
        // Display error message
        submitButton.disabled = true;
        Toastify({
            text: "All fields are required. Please enter all fields.",
            duration: 4000, // duration in milliseconds
            gravity: 'top', // 'top' or 'bottom'
            position: 'right', // 'left', 'center', or 'right'
            backgroundColor: 'red',
            style: {
                top: '-100px', // Adjust this value based on your desired top margin
            },
        }).showToast();

        // Disable the submit button
       
    } else {
        // Enable the submit button
        submitButton.disabled = false;

        // Proceed to the next form or other actions
    }
});


//_________________________________________________________________________________________________________________________________________________________________


// This is Function Show  Form One or Two Comndition wise click on Next Button and Privous Buttton
//1) Account / Registor


//Next Button  Show Form2
function showNextForm() {
    const form1 = document.querySelector('.form1');



    const form2 = document.querySelector('.form2');

    form1.style.display = 'none';
    form2.style.display = 'block';
}
//Privous Button Show Form1
function showPreviousForm() {
    const form1 = document.querySelector('.form1');
    const form2 = document.querySelector('.form2');

    form1.style.display = 'block';
    form2.style.display = 'none';
}





//_________________________________________________________________________________________________________________________________________________________________

//Upload Adhar and Pan Card Validations 

var isValidPdfAadharFile = false;
var isValidPdfPANFile = false;
//Adhar Card
document.getElementById('fileAdharInput').addEventListener('change', function () {
    const fileInput = this;
    const errorContainer = document.getElementById('fileNameAdharDisplay');
    var submitButton = document.getElementById("registerButton");

    // Check if a file was selected
    if (fileInput.files.length > 0) {
        const selectedFile = fileInput.files[0];
        const validExtensions = ['.pdf', '.jpg', '.jpeg']; // Separate extensions with commas

        const fileExtension = selectedFile.name.split('.').pop().toLowerCase();
        const isValidExtension = validExtensions.includes('.' + fileExtension);

        if (!isValidExtension) {
            $('#fileNameAdharDisplay').text('Please select a valid pdf, jpg, or jpeg file.').css('color', 'red');
            isValidPdfAadharFile = false;

            if (isValidPdfAadharFile == false || isValidPdfPANFile == false) {
                submitButton.disabled = true;
                fileInput.value = ''; // Clear the file input
            }
        } else {
            if (fileInput.files.length > 0) {
                console.log("after file name displayed");
                $('#fileNameAdharDisplay').text("Selected File: " + fileInput.files[0].name).css('color', 'green');
                isValidPdfAadharFile = true;

                if (isValidPdfAadharFile == true && isValidPdfPANFile == true) {
                    submitButton.disabled = false;
                }
            } else {
                $('#fileNameAdharDisplay').text('No file selected').css('color', 'red');
            }
        }
    }
});


//Pan Card
 document.getElementById('fileInput').addEventListener('change', function () {
            const fileInput = this;
            const errorContainer = document.getElementById('fileNameDisplay');
            var submitButton = document.getElementById("registerButton");

            // Check if a file was selected
            if (fileInput.files.length > 0) {
                const selectedFile = fileInput.files[0];
                const validExtensions = ['.pdf', '.jpg', '.jpeg']; // Separate extensions with commas

                const fileExtension = selectedFile.name.split('.').pop().toLowerCase();
                const isValidExtension = validExtensions.includes('.' + fileExtension);

                if (!isValidExtension) {
                    $('#fileNameDisplay').text('Please select a valid pdf, jpg, or jpeg file.').css('color', 'red');
                    isValidPdfPANFile = false;

                    if (isValidPdfAadharFile == false || isValidPdfPANFile == false) {
                        submitButton.disabled = true;
                        fileInput.value = ''; // Clear the file input
                    }
                } else {
                    if (fileInput.files.length > 0) {
                        console.log("after file name displayed");
                        $('#fileNameDisplay').text("Selected File:" + fileInput.files[0].name).css('color', 'green');
                        isValidPdfPANFile = true;

                        if (isValidPdfAadharFile == true && isValidPdfPANFile == true) {
                            submitButton.disabled = false;
                        }
                    } else {
                        $('#fileNameDisplay').text('No file selected').css('color', 'red');
                    }
                }
            }
        });


//_________________________________________________________________________________________________________________________________________________________________
//Check Email Id and Gst No is Exist or not Validations

// Email Check
function checkEmail() {
    var email = document.getElementById('emailInput').value.trim();
    var submitButton = document.getElementById("registerButton");
    var nextButton = document.getElementById('nextButton');

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
                    nextButton.disabled = false;

                } else {
                    $('#resultMessage').text('This email is already taken. Please enter a new email.').css('color', 'red');
                    submitButton.disabled = true;
                    nextButton.disabled = true;

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

//GST No Check

function checkGstNo() {
    var GstNo = document.getElementById('Gstno').value.trim();
    var submitButton = document.getElementById("registerButton");

    if (GstNo) {

        console.log(GstNo);

        $.ajax({
            type: 'POST',
            url: '/UserDetails/CheckGstNo',
            data: { GstNo: GstNo },
            success: function (result) {
                console.log(result);
                if (result) {
                    $('#resultMessage1').text('').css('color', 'green');
                    submitButton.disabled = false;

                } else {
                    $('#resultMessage1').text('This Gst no is already Exist. Please Uniqe Gst No Enter.').css('color', 'red');
                    submitButton.disabled = true;

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



//_________________________________________________________________________________________________________________________________________________________________

function restrictLength(input, maxLength) {
    const value = input.value.toString(); // Convert the numeric value to a string
    if (value.length > maxLength) {
        input.value = value.slice(0, maxLength); // Trim the value to the maximum length
    }
}

//_________________________________________________________________________________________________________________________________________________________________

var loadFile = function (event) {
    var image = document.getElementById("output");
    image.src = URL.createObjectURL(event.target.files[0]);
};