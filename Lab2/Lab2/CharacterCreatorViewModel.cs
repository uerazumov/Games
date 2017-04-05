using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2
{
    class CharacterCreatorViewModel
    {
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
                "Файтер",
                "Клирик",
                "Вор",
                "Маг"
            };
        }
    }
}
