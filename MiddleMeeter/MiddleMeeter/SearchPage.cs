﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace MiddleMeeter {
  enum SearchMode {
    coffee = 0,
    food = 1,
    drinks = 2,
  }

  class ModeConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
      return (int)value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
      return (SearchMode)value;
    }
  }

  class SearchModel : INotifyPropertyChanged {
    string yourLocation;
    string theirLocation;
    SearchMode mode = SearchMode.food;

    public string YourLocation {
      get { return this.yourLocation; }
      set {
        if (this.yourLocation != value) {
          this.yourLocation = value;
          NotifyPropertyChanged();
        }
      }
    }

    public string TheirLocation {
      get { return this.theirLocation; }
      set {
        if (this.theirLocation != value) {
          this.theirLocation = value;
          NotifyPropertyChanged();
        }
      }
    }

    public SearchMode Mode {
      get { return this.mode; }
      set {
        if (this.mode != value) {
          this.mode = value;
          NotifyPropertyChanged();
        }
      }
    }

    void NotifyPropertyChanged([CallerMemberName]string propertyName = "") {
      if (PropertyChanged != null) {
        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;
  }

  class SearchPage : ContentPage {
    SearchModel model = new SearchModel();

    public SearchPage() {
      Title = "Search";
      Padding = 20;
      BindingContext = model;

      var button1 = new Button { Text = "Search", HorizontalOptions = LayoutOptions.End, WidthRequest = 200 };
      button1.Clicked += button1_Clicked;

      var picker = new Picker { Title = "Mode" };
      foreach (var mode in new string[] { "coffee", "food", "drinks" }) {
        picker.Items.Add(mode);
      }
      picker.SetBinding(Picker.SelectedIndexProperty, new Binding("Mode", converter: new ModeConverter()));

      var yourLocation = new Entry { Placeholder = "your location" };
      yourLocation.SetBinding(Entry.TextProperty, new Binding("YourLocation"));

      var theirLocation = new Entry { Placeholder = "their location" };
      theirLocation.SetBinding(Entry.TextProperty, new Binding("TheirLocation"));

      Content = new StackLayout {
        Children = {
          new Label { Text = "Your Location:" },
          yourLocation,
          new Label { Text = "Their Location:" },
          theirLocation,
          new Label { Text = "Mode:" },
          picker,
          button1,
        }
      };
    }

    void button1_Clicked(object sender, EventArgs e) {
      Debug.WriteLine("Search parameters:");
      Debug.WriteLine("\tYourLocation= " + model.YourLocation);
      Debug.WriteLine("\tTheirLocation= " + model.TheirLocation);
      Debug.WriteLine("\tMode= " + model.Mode);

      var results = new Result[] {
        new Result { Name = "Starbucks", Description = "some coffee" },
        new Result { Name = "Seattle's Best Coffee", Description = "some more coffee" },
        new Result { Name = "Dutch Bros.", Description = "still more coffee" },
      };
      Navigation.PushAsync(new ResultsPage(results));
    }

  }
}