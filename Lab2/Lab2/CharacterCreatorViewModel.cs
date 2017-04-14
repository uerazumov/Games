using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Lab2
{
    class CharacterCreatorViewModel
    {
        public ICommand Cancel { get; set; }
        public ICommand Save { get; set; }
        public byte Force { get; set; }
        public byte Agility { get; set; }
        public byte Intelligence { get; set; }
        public byte Luck { get; set; }
        public byte FreePoints { get; set; }
        public string Nickname { get; set; }
        public List<string> CharacterClass { get; set; }
        public string SelectedCharacterClass { get; set; }
        public CharacterCreatorViewModel()
        {
            CharacterClass = new List<string>()
            {
                "Саппорт",
                "Файтер",
                "Клирик",
                "Стрелок",
                "Маг"
            };
            FreePoints = 10;
        }
    }
}
