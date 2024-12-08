namespace APIOpenWeather.Pages;

public partial class InfoPage : ContentPage
{
    public InfoPage()
	{
		InitializeComponent();
    }

    private void OnContinentPickerChanged(object sender, EventArgs e)
    {
        var continent = ContinentPicker.SelectedItem.ToString();
        string info = continent switch
        {
            "Europe" => "Europe is known for its temperate climate, with milder temperatures, especially in the western region where Atlantic winds keep temperatures stable. Southern Europe tends to be warmer, with hot summers and mild winters, while the north experiences a colder, harsher climate.",
            "Africa" => "Africa is characterized by hot, arid climates with vast desert regions like the Sahara. The equatorial regions, like Central Africa, have high humidity and frequent rains, while the southern parts of the continent experience a more temperate climate with intense torrential rains during certain periods.",
            "Asia" => "Asia has a wide variety of climates. Southeast Asia is known for its tropical climate with intense rains during the monsoon season, while northern regions like Siberia experience harsh winters with extreme cold temperatures. East Asia also has temperate climates with significant seasonal variations.",
            "Oceania" => "Oceania's climate varies from tropical to temperate. In Australia, the interior is extremely hot, while the coast has milder temperatures. New Zealand has a temperate, wet climate with a strong influence from the Pacific Ocean.",
            "Americas" => "The Americas feature a wide range of climates. The north has cold and temperate climates, while Central and South America are warmer, with tropical and subtropical climates. The Amazon region faces high temperatures and intense rainfall, while areas like the Atacama Desert in Chile are extremely dry.",
            _ => "Select a continent to see temperature trends and climate information."
        };

        ContinentInfoLabel.Text = info;

    }
}