namespace MOL3;

/*
 * addd the mole, move the mole, get a score
 * reset the board
 * what about multiple moles
 * add a timer. stopwatch, system.threading.timer
 * system.timers.time. maui has a periodic timer
 * on vs19 use device timer
 * on vs22 use dispatcher
 * start timer and the application runs piece of code when the system 
 * generates tick event.
 */

//use a system.timers.timer time.
//define an interval
//explicit start and stop method
//still works on a seperate thread
//primary thread - ui thread
//timer thread - non ui threadin the background
//as a result
//timer sends a message to the application - interrupt(tick)
//application provides a piece of code to be run
//method to be run in response to the interrupt.
//to pass control back, use the anonymous method
// method() {CODE GOES HERE	}   "() => {}"

public partial class MainPage : ContentPage
{
	Random _random;
	bool _IsTimerRunning;
	System.Timers.Timer _timer;
	int _MyInterval = 500;
	int CountDownStart = 30;
	public MainPage()
	{
		InitializeComponent();
		_random = new Random(); //instantiate the object
		SetUpGorilla();
		SetUpTimers();
	}

	private void SetUpTimers()
	{
		//use flobal variable for timer
		_timer = new System.Timers.Timer(); //instantiate
		_timer.Interval = _MyInterval;
		//to add an event handler, use the +=
		//i += 1
		_timer.Elapsed += _timer_Elapsed1;
		CountDownStart = 30;
		LblCounter.Text = CountDownStart.ToString();
	}

	private void _timer_Elapsed1(object sender, System.Timers.ElapsedEventArgs e)
	{
		//ask the ui to update some stuff - through a method
		// ask the dispatcher to kick off the method on the ui thread
		//this method is on the timer thread
		Dispatcher.Dispatch(
			() => {
				UpdateAfterTimer();		
			}
			);
	}

	private void UpdateAfterTimer()
	{
		//back on the ui thread
		int counter;
		//retrieve text from the screen
		counter = Convert.ToInt32(LblCounter.Text);
		counter--;
		LblCounter.Text = counter.ToString();
		if(counter < 6)
		{
			LblCounter.TextColor = Colors.Red;
		}
		if(counter==0)
		{
			_timer.Stop();
			//reset button text as well
			Timer.Text = "Start";
			
		}
	}

	private void SetUpGorilla()
	{
		int r = 0, c = 0, maxR = 3;
		BoxView b;

		//declare gesture recogniser item
		TapGestureRecognizer t = new TapGestureRecognizer();
		t.Tapped += BoxviewTapGR_Tapped;

		for (r = 0; r < maxR; r++)
		{
			for (c = 0; c < maxR; c++)
			{
                b = new BoxView();
                b.HorizontalOptions = LayoutOptions.Center;
                b.VerticalOptions = LayoutOptions.Center;
                b.HeightRequest = 100;
                b.WidthRequest = 100;
                b.CornerRadius = 50;
               // b.Color = Colors.Yellow;
				b.IsVisible = false;
                // add the gesture recogniser.
                // add the event handler
                b.GestureRecognizers.Add(t);
                // attached property (shared)
                b.SetValue(Grid.RowProperty, r);
                b.SetValue(Grid.ColumnProperty, c);
				GameGrid3x3.Children.Add(b);
            }
        }
	}


    private void BoxviewTapGR_Tapped(
        object sender, EventArgs e)
	{
		//change coor to blue
		//to use the sender, cast it to what you want
		((BoxView)sender).IsVisible = false;
		((BoxView)sender).Color = Colors.Blue;
	}

	private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
	{

	}

	private void BtnChangeGrid_Clicked(object sender, EventArgs e)
	{
		//switch statement
		switch (BtnChangeGrid.Text)
		{
			case "3x3":
				{
					GameGrid3x3.IsVisible = true;
					GameGrid5x5.IsVisible = false;
					BtnChangeGrid.Text = "3x3";
					break;
				}
			case "5x5":
				{
                    GameGrid3x3.IsVisible = false;
                    GameGrid5x5.IsVisible = true;
                    BtnChangeGrid.Text = "5x5";
                    break;
				}
		}
		ResetTheGame();	

		
	}

	private void ImageButton_Clicked(object sender, EventArgs e)
	{
		//make image button disappear
		// want to reus this event handler
		//moles - same functionality
		// add a score for the user, then disappear
		//do the score on a label
		int score = Convert.ToInt32(LblScore.Text);
		score += 10;
		LblScore.Text = score.ToString();

		//use the sender
		ImageButton ib = (ImageButton)sender;
		ib.IsVisible = false;
	}

	private void ResetTheGame()
	{
        foreach (var item in GameGrid3x3.Children)
        {
            ImageButton imageButton;
            if (item.GetType() == typeof(ImageButton))
            {
                imageButton = (ImageButton)item;
                imageButton.IsVisible = true;
            }
        }

        // reset the score back to 0
        LblScore.Text = "0";
    }

	private void BtnTimer_Clicked(object sender, EventArgs e)
	{
		//move the mole image around the grid
		//MoveTheMole();
		//create a start timer
		//time interval in miliseconds
		int msTime = 1000;
		_IsTimerRunning = true;
		//if player score is over 100
		if (Convert.ToInt32(LblScore.Text) > 40)
		{
			_IsTimerRunning = false;
		};
		//change the timespan value. mstime *=0.75;
		Dispatcher.StartTimer(TimeSpan.FromMilliseconds(1000),
			() => {
				Dispatcher.Dispatch(
					() => { MoveTheMole(); });
				return _IsTimerRunning;
					});
	}
	private void MoveTheMole()
	{
		int MaxRow, MaxColumn;
		ImageButton currentImageButton;
		//whihc grid is visible
		//and decide which mole to move
		if(GameGrid3x3.IsVisible ==  true)
		{
			MaxRow = MaxColumn = 3;
			currentImageButton = Mole1;
			// get random number for r, c
			//move mole
			//make mole visible
		}
		else
		{
            MaxRow = MaxColumn = 5;
            currentImageButton = Mole2;
            // get random number for r, c
            //move mole
            //make mole visible
        }

		//set the row and column value based on the size
		int r = _random.Next(0, MaxRow);
		int c = _random.Next(0, MaxColumn);
		//move the appropriate mole image
		currentImageButton.SetValue(Grid.RowProperty, r);
		currentImageButton.SetValue(Grid.ColumnProperty, c);
		//make the mole visible
		currentImageButton.IsVisible = true;
		//add timer in last so you know code works 

	}

	private void Timer_Clicked(object sender, EventArgs e)
	{
		if (Timer.Text == "Start")
		{
            LblCounter.Text = CountDownStart.ToString();
            LblCounter.TextColor = Colors.Blue;
			Timer.Text = "Stop";
			_timer.Start();
		}
		else if(Timer.Text == "Stop")
		{
			Timer.Text = "Start";
			_timer.Stop();
		}
	}
}


