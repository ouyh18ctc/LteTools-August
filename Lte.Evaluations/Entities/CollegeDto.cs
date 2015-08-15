using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;
using Lte.Domain.Geo.Abstract;
using Lte.Domain.Regular;
using Lte.Parameters.Entities;
using Lte.Parameters.Service.Public;

namespace Lte.Evaluations.Entities
{
    public class CollegeDto : AuditedEntityDto, IValidate, ITown
    {
        public int TownId { get; set; }

        [Display(Name = "校园名称")]
        [Required]
        public string Name { get; set; }

        [Display(Name = "在校生总数")]
        [Required]
        [Range(100, 50000)]
        public int TotalStudents { get; set; }

        [Display(Name = "天翼用户数")]
        [Required]
        [Range(0, 50000)]
        [CompareValues("TotalStudents", CompareValues.LessThanOrEqualTo, ErrorMessage = "天翼用户数不能超过在校学生数")]
        public int CurrentSubscribers { get; set; }

        [Display(Name = "毕业天翼用户数")]
        [Required]
        [Range(0, 50000)]
        [CompareValues("CurrentSubscribers", CompareValues.LessThanOrEqualTo,
            ErrorMessage = "毕业天翼用户数不能超过天翼用户数")]
        public int GraduateStudents { get; set; }

        [Display(Name = "预计放号数（个）")]
        [Required]
        [Range(0, 50000)]
        public int NewSubscribers { get; set; }

        [Display(Name = "老生开学时间")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime OldOpenDate { get; set; }

        [Display(Name = "新生开学时间")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime NewOpenDate { get; set; }

        [Display(Name = "预计用户到达数")]
        public int ExpectedSubscribers
        {
            get { return CurrentSubscribers + NewSubscribers - GraduateStudents; }
        }

        [Display(Name = "城市")]
        public string CityName { get; set; }

        [Display(Name = "区域")]
        public string DistrictName { get; set; }

        [Display(Name = "镇区")]
        public string TownName { get; set; }

        public CollegeDto()
        {
            CityName = "佛山";
            DistrictName = "高明";
            TownName = "富湾";
            Name = "";
            TotalStudents = 5000;
            CurrentSubscribers = 4000;
            NewSubscribers = 1500;
            GraduateStudents = 1000;
            OldOpenDate = DateTime.Today.AddDays(50);
            NewOpenDate = DateTime.Today.AddDays(80);
            Id = -1;
        }

        public CollegeDto(CollegeInfo info, IEnumerable<Town> towns)
        {
            info.CloneProperties(this);
            Town town = towns.FirstOrDefault(x => x.Id == info.TownId);
            if (town != null)
            {
                CityName = town.CityName;
                DistrictName = town.DistrictName;
                TownName = town.TownName;
            }
        }
    }

    public class CreateCollegeInput : IInputDto
    {
        public CollegeDto College { get; set; }
    }

    public class CreateCollegeOutput : IOutputDto
    {
        public CollegeDto College { get; set; }
    }

    public class DeleteCollegeInput : IInputDto
    {
        public int Id { get; set; }
    }

    public class DeleteCollegeOutput : IOutputDto
    {
        public int Id { get; set; }
    }

    public class GetCollegeInput : IInputDto
    {
        public int Id { get; set; }
    }

    public class GetCollegeOutput : IOutputDto
    {
        public CollegeDto College { get; set; }
    }
}
