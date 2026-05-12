using FluentValidation;
using QuickServePOS.Models.DTO.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Models.ValidationModels.APISideValidation.CommonValidation
{
    public class UploadProfilePhotoValidation : AbstractValidator<UploadProfileImageDto>
    {
        public UploadProfilePhotoValidation()
        {
            RuleFor(x => x.Image)
                .Must(file =>
                {
                    if (file == null)
                        return false;

                    var allowed =
                        new[] { ".jpg", ".jpeg", ".png" };

                    var extension =
                        Path.GetExtension(file.FileName)
                        .ToLower();

                    return allowed.Contains(extension);
                })
                .WithMessage(
                    "Only JPG and PNG are allowed");

            RuleFor(x => x.Image)
                .Must(file =>
                    file != null &&
                    file.Length <= 2 * 1024 * 1024)
                .WithMessage(
                    "Maximum file size is 2 MB");
        
        }
    
    }
    
}
