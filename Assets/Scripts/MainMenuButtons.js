function PlayGame(){
	//PlayerPrefs.DeleteAll();

	if(PlayerPrefs.HasKey("map")){
	Application.LoadLevel(PlayerPrefs.GetInt("map"));
		
	} else {
	Application.LoadLevel(1);
	}


}

function QuitGame() {
	Application.Quit();
}