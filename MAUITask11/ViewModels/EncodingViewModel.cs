using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MAUITask11.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MAUITask11.ViewModels
{
    public partial class EncodingViewModel : ObservableObject
    {

        public EncodingViewModel()
        {
            Genders = new List<ItemPicker<Gender>> { new(Gender.Woman, "Woman"), new (Gender.Man, "Man") };
        }
        [ObservableProperty]
        private DateTime _birthDay;


        public List<ItemPicker<Gender>> Genders { get; } 

        [ObservableProperty]
        private ItemPicker<Gender>? _selectedGender;

        [RelayCommand]
        public async Task GeneratePESEL()
        {

        }

        [RelayCommand]
        public async Task CopyToClipboard()
        {

        }
    }
}
