using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lab2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public String Text { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }
        //Реализация кнопки "Сохранить персонажа"
        public List<string> SqlCharacterNames = new List<string>();
        private void SaveCharacter(object sender, RoutedEventArgs e)
        {
            using (SqlConnection CharTable = new SqlConnection("Server=DESKTOP-AUTQL9N;Database=myDataBase;Trusted_Connection=True;"))
            {
                CharTable.Open();
                SqlCommand cmd = new SqlCommand("SELECT s.Nickname FROM Characters s", CharTable);
                var CharactersReader = cmd.ExecuteReader();
                while (CharactersReader.Read())
                {
                    String S = (String)CharactersReader["Nickname"];
                    SqlCharacterNames.Add(S);
                }
                int i = 0;
                while ((i < SqlCharacterNames.Count)&&(i >= 0))
                {
                    if (Nick.Text == SqlCharacterNames[i])
                    {
                        MessageBox.Show("Данное имя уже занято!");
                        i = -1;
                    }
                    if (i != -1)
                    {
                        i++;
                    }
                }
                CharactersReader.Close();
                if (i != -1)
                {
                    SqlCommand save = new SqlCommand("AddNewCharacter", CharTable);
                    save.CommandType = System.Data.CommandType.StoredProcedure;
                    if ((Nick.Text != "")&&(Nick.Text != null))
                    {
                        SqlParameter NewNickname = new SqlParameter("@NickName", Nick.Text);
                        SqlParameter NewClass = new SqlParameter("@Class", ClassBox.SelectedItem);
                        SqlParameter NewForce = new SqlParameter("@Force", ForceBox.Value);
                        SqlParameter NewAgility = new SqlParameter("@Agility", AgilityBox.Value);
                        SqlParameter NewIntellegence = new SqlParameter("@Intelligence", IntelligenceBox.Value);
                        SqlParameter NewLuck = new SqlParameter("@Luck", LuckBox.Value);
                        SqlParameter NewFreePoints = new SqlParameter("@FreePoints", FreePointsBox.Text);
                        save.Parameters.Add(NewNickname);
                        save.Parameters.Add(NewClass);
                        save.Parameters.Add(NewForce);
                        save.Parameters.Add(NewAgility);
                        save.Parameters.Add(NewIntellegence);
                        save.Parameters.Add(NewLuck);
                        save.Parameters.Add(NewFreePoints);
                        var SaveReader = save.ExecuteReader();
                        MessageBox.Show("Персонаж успешно сохранён");
                    }
                    else
                    {
                        MessageBox.Show("Укажите имя персонажа!");
                    }
                }
            }
        }
    }
}
