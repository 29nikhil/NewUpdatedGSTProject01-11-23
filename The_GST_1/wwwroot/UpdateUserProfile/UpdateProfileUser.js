document.addEventListener('DOMContentLoaded', function () {
    var nextButton = document.getElementById('nextButtonUpdateView');
    console.log("Next Buttton Hite 1 ")
    nextButton.addEventListener('click', function (event) {


        
        // Perform null check for all fields in the current form
        var FirstName = document.getElementById('exampleFirstName').value;
        var MiddleName = document.getElementById('exampleMiddleName').value;

        var LastName = document.getElementById('exampleLastName').value;
        var PhoneNo = document.getElementById('examplePhoneName').value;
        var Email = document.getElementById('emailInput').value;
        var City = document.getElementById('exampleCityName').value;
        var Country = document.getElementById('exampleCountry').value;
        var Address = document.getElementById('exampleAddress').value;



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
        else if (validatePhone(PhoneNo)) {
            console.log("Phone is valid!");
        }
        else {
            errorMessage = 'Please  Number is required.\n';
            var errorContainer = document.getElementById('PhoneNumberError');
            errorContainer.innerHTML = errorMessage === '' ? '' : errorMessage;
        }


        
       

        

        if (!Email) {
            errorMessage = 'Email is required.\n';
            var errorContainer = document.getElementById('EmailError');
            errorContainer.innerHTML = errorMessage === '' ? '' : errorMessage;
        }
        else if (validateEmail(Email)) {
            console.log("Email is valid!");

        }
      
        else {
            errorMessage = 'Email Format Wrong is required.\n';
            var errorContainer = document.getElementById('EmailError');
            errorContainer.innerHTML = errorMessage === '' ? '' : errorMessage; // Enable the submit button
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


//----------------------------------------------------------------------------------------------------------------------------
//Email Validation
function validateEmail(email) {
    console.log("Email Validated");
    var emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailRegex.test(email);
}
//Phone No Validation
function validatePhone(phone) {
    console.log("Phone No Validated");
    // Validate if the input is a valid number and doesn't start with zero (optional: and has 10 digits)
    return !isNaN(phone) && phone.length === 10 && phone.charAt(0) !== '0';
}

//----------------------------------------------------------------------------------------------------------------------------
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
    if (!BusinessType || !PanNo || !AdharNo || !Website || !GstNo) {
        // Display error message

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


//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
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

                const selectedFile = fileInput.files[0];
                if (fileExtension == "pdf") {

                }
                else {
                    displayImagePreview(selectedFile);
                }
                displayImagePreview(selectedFile);

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
//_________________________________________
//View Adhar Card File
function displayImagePreview(file) {
    var imageContainer = document.getElementById("Adharoutput");
    var fileExtension = file.name.split('.').pop().toLowerCase();

    if (fileExtension === 'pdf') {
        var objectElement = document.createElement('object');
        objectElement.data = URL.createObjectURL(file);
        objectElement.type = 'application/pdf';
        //objectElement.width = '100%';
        //objectElement.height = '100px';

        var icon = document.createElement('img');
        icon.src = 'https://www.precious-artgold.com/wp-content/uploads/2016/09/PDF-icon-1.png'; // Replace with the path to your PDF icon
        icon.alt = 'PDF Icon';
        icon.style.maxWidth = '100px'; // Adjust the size of the icon as needed

        var openLink = document.createElement('a');
        openLink.href = URL.createObjectURL(file);
        openLink.target = '_blank';
        openLink.textContent = 'Open in a new tab';

        var viewButton = document.createElement('button');
        viewButton.innerHTML = 'View <i class="fa-solid fa-eye"></i>';

        // Adding styles to the button
        viewButton.style.backgroundColor = 'blue';
        viewButton.style.color = 'white';
        viewButton.style.padding = '4% 5%'; // Using percentage-based padding
        viewButton.style.border = 'none';
        viewButton.style.borderRadius = '5px';
        viewButton.style.paddingLeft = '7%'; // Using percentage-based padding
        viewButton.setAttribute('tooltip', 'View Document.');
        viewButton.setAttribute('flow', 'up');

        // Adding a media query for smaller screens
        var mediaQuery = window.matchMedia('(max-width: 600px)');
        if (mediaQuery.matches) {
            // Adjust styles for smaller screens
            viewButton.style.padding = '8px 12px';
            viewButton.style.paddingLeft = '25px';
        }

        viewButton.style.cursor = 'pointer';
        viewButton.onclick = function () {
            //this Function Avoid Submit Form Event.preventDefault
            event.preventDefault();
            window.open(URL.createObjectURL(file), '_blank');
        };

        imageContainer.innerHTML = '';
        /* imageContainer.appendChild(objectElement);*/
        imageContainer.appendChild(icon);
        /*  imageContainer.appendChild(openLink);*/
        imageContainer.appendChild(viewButton);
    } else {
        // Display image using <img>
        // Display image using <img>
        var imgElement = document.createElement('img');
        imgElement.src = URL.createObjectURL(file);
        imgElement.alt = 'Pan Image';
        imgElement.style.maxWidth = '200px';
        imgElement.style.maxHeight = '120px';
        imgElement.style.borderRadius = '7px';
        imgElement.style.border = 'dashed';
        imgElement.style.backgroundSize = '3px';
        imgElement.style.backgroundSize = '3px';
        imgElement.style.borderColor = 'black';
        imgElement.height = '110';
        imgElement.width = '200';

        imageContainer.innerHTML = '';
        imageContainer.appendChild(imgElement);
    }
}

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

                const selectedFile = fileInput.files[0];
                if (fileExtension == "pdf") {

                }
                else {
                    displayImagePan(selectedFile);
                }
                displayImagePan(selectedFile);


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

//_________________________________________
//View Pan Card File

function displayImagePan(file) {
    var imageContainer = document.getElementById("Panoutput1");
    var fileExtension = file.name.split('.').pop().toLowerCase();
    if (fileExtension === 'pdf') {
        console.log("PAn Card View Pdf File")
        var objectElement = document.createElement('object');
        objectElement.data = URL.createObjectURL(file);
        objectElement.type = 'application/pdf';
        //objectElement.width = '100%';
        //objectElement.height = '100px';

        var icon = document.createElement('img');
        icon.src = 'https://www.precious-artgold.com/wp-content/uploads/2016/09/PDF-icon-1.png'; // Replace with the path to your PDF icon
        icon.alt = 'PDF Icon';
        icon.style.maxWidth = '100px'; // Adjust the size of the icon as needed

        var openLink = document.createElement('a');
        openLink.href = URL.createObjectURL(file);
        openLink.target = '_blank';
        openLink.textContent = 'Open in a new tab';

        var viewButton = document.createElement('button');
        viewButton.innerHTML = 'View <i class="fa-solid fa-eye"></i>';

        // Adding styles to the button
        viewButton.style.backgroundColor = 'blue';
        viewButton.style.color = 'white';
        viewButton.style.padding = '4% 5%'; // Using percentage-based padding
        viewButton.style.border = 'none';
        viewButton.style.borderRadius = '5px';
        viewButton.style.paddingLeft = '7%'; // Using percentage-based padding

        viewButton.setAttribute('tooltip', 'View Document.');
        viewButton.setAttribute('flow', 'up');


        // Adding a media query for smaller screens
        var mediaQuery = window.matchMedia('(max-width: 600px)');
        if (mediaQuery.matches) {
            // Adjust styles for smaller screens
            viewButton.style.padding = '8px 12px';
            viewButton.style.paddingLeft = '25px';
        }

        viewButton.style.cursor = 'pointer';
        viewButton.onclick = function () {
            event.preventDefault();
            window.open(URL.createObjectURL(file), '_blank');
        };

        imageContainer.innerHTML = '';
        /* imageContainer.appendChild(objectElement);*/
        imageContainer.appendChild(icon);
        /*  imageContainer.appendChild(openLink);*/
        imageContainer.appendChild(viewButton);
    } else {
        // Display image using <img>
        // Display image using <img>
        var imgElement = document.createElement('img');
        imgElement.src = URL.createObjectURL(file);
        imgElement.alt = 'Pan Image';
        imgElement.style.maxWidth = '200px';
        imgElement.style.maxHeight = '120px';
        imgElement.style.borderRadius = '7px';
        imgElement.style.border = 'dashed';
        imgElement.style.backgroundSize = '3px';
        imgElement.style.backgroundSize = '3px';
        imgElement.style.borderColor = 'black';
        imgElement.height = '110';
        imgElement.width = '200';

        imageContainer.innerHTML = '';
        imageContainer.appendChild(imgElement);
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
//Change Dp By Upload 
var loadFile = function (event) {
    var image = document.getElementById("output");
    image.src = URL.createObjectURL(event.target.files[0]);
};




function checkEmail() {
    var email = document.getElementById('emailInput').value.trim();
    var userid = document.getElementById('userId').value.trim();

    var nextButton = document.getElementById('nextButtonUpdateView');
    var submitButton = document.getElementById("registerButton");

    if (email) {



        $.ajax({
            type: 'POST',
            url: '/UserDetails/CheckEmail',
            data: { email: email, userid: userid },
            success: function (result) {
                console.log(result);
                if (result) {
                    $('#resultMessage').text('Email is available.').css('color', 'green');
                    submitButton.disabled = false;
                    nextButton.disabled = false;

                } else {
                    $('#resultMessage').text('This email is already taken. Please enter a new email.').css('color', 'red');
                    nextButton.disabled = true;

                    event.preventDefault();
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






function restrictLengthUpdateProfile(input, maxLength) {
    var nextButton = document.getElementById('nextButtonUpdateView');
    nextButton.disabled = false;

    const value = input.value.toString(); // Convert the numeric value to a string
    if (value.length > maxLength) {
        console.log("Value trimmed to the maximum length Profile update");

        input.value = value.slice(0, maxLength);
        nextButton.disabled = false;
        // Trim the value to the maximum length
    }

}