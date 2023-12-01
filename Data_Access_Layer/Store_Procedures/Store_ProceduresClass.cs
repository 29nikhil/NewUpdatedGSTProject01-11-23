using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer.Store_Procedures
{
    public class Store_ProceduresClass
    {

        public string DeleteUserProcedure { get; } = @"CREATE PROCEDURE [dbo].[Delete_User] @Userid VARCHAR(100)  AS
                                                  BEGIN
                                                             DELETE FROM UserDetails WHERE UserId = @Userid;
                                                            DELETE FROM AspNetUsers WHERE Id = @Userid;
                                                  END";


        public string EditUserProcedure { get; } = @"CREATE PROCEDURE [dbo].[Edit_User] 

                                                     @Userid VARCHAR(100),
                                                    @FirstName VARCHAR(100),
                                                    @MiddleName VARCHAR(100),
                                                    @LastName VARCHAR(100),
                                                    @PhoneNumber VARCHAR(100),
                                                    @Address VARCHAR(100),
                                                    @Country VARCHAR(100),
                                                    @city VARCHAR(100),
                                                    @UserStatus VARCHAR(100),
                                                    @Email VARCHAR(100), 
                                                    @gstNo VARCHAR(100),
                                                    @PanNo VARCHAR(100),
                                                    @UploadPan VARCHAR(100),
                                                    @UploadAdhar VARCHAR(100),
                                                    @WebSite VARCHAR(100),
                                                    @Bussiness VARCHAR(100),
                                                    @AdharNo VARCHAR(12)
                                             AS
                                             BEGIN
                                                   UPDATE AspNetUsers
                                                   SET FirstName = @FirstName,
                                                   MiddleName = @MiddleName,
                                                   LastName = @LastName,
                                                   UserName = @Email,
                                                   NormalizedEmail = UPPER(@Email),
                                                   NormalizedUserName = UPPER(@Email),
                                                   PhoneNumber = @PhoneNumber,
                                                   Address = @Address,
                                                   Country = @Country,
                                                   city = @city,
                                                   UserStatus = @UserStatus,
                                                   Email = @Email
                                                
                                                  WHERE Id = @Userid;
    
                                                UPDATE UserDetails
                                                SET GSTNo = @gstNo,
                                                PANNo = @PanNo,
                                                UploadAadhar = @UploadAdhar,
                                                UploadPAN = @UploadPan,
                                                website = @WebSite,
                                                BusinessType = @Bussiness,
                                                AdharNo = @AdharNo
                                                WHERE UserId = @Userid;
                                            END";

        public string EditFellowshipProcedure { get; } = @"CREATE PROCEDURE [dbo].[Edit_FellowShip1]
                                                   @Userid VARCHAR(100),
                                                  @FirstName VARCHAR(100),
                                           @MiddleName VARCHAR(100),
            @LastName VARCHAR(100),
            @PhoneNumber VARCHAR(100),
            @Address VARCHAR(100),
            @Country VARCHAR(100),
            @UserStatus VARCHAR(100),
            @Email VARCHAR(100),
            @city VARCHAR(100)
        AS
        BEGIN
            UPDATE AspNetUsers
                SET FirstName = @FirstName,
                    MiddleName = @MiddleName,
                    LastName = @LastName,
                    PhoneNumber = @PhoneNumber,
                    UserName = @Email,
                    NormalizedEmail = UPPER(@Email),
                    NormalizedUserName = UPPER(@Email),
                    Address = @Address,
                    Country = @Country,
                    city = @city,
                    UserStatus = @UserStatus,
                    Email = @Email
            WHERE Id = @Userid;

            UPDATE userResistorLogs
                SET RegistorByUserName = CONCAT(@FirstName, ' ', @LastName),
                    RegistorByEmail = @Email,
                    ModifiedDate = GETDATE()
            WHERE RegistorById = @Userid;
        END";
        public string EmailStatus { get; } = @"
            CREATE PROCEDURE [dbo].[EmailConfirm]
                @UserId VARCHAR(100),
                @EmailConfirmation BIT
            AS
            BEGIN
                UPDATE AspNetUsers
                    SET EmailConfirmed = @EmailConfirmation
                WHERE Id = @UserId;
            END
";

    }
}
