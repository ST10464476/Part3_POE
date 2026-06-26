using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using Part3_POE.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Part3_POE.Services;

namespace Part3_POE;

public partial class WindowGame : Window, INotifyPropertyChanged
{
    // Private fields
    private string _questionNumberString;
    private string _question;
    private string _option1;
    private string _option2;
    private string _option3;
    private string _option4;
    private string _scoreString;
    private ICommand _buttonPressed;
    private bool _isSubmitEnabled;
    private Questions _quizQuestions;
    private int _currentQuestionIndex = 0;
    private int _score = 0;

    // Radio button selection properties
    private bool _selectedOption1;
    private bool _selectedOption2;
    private bool _selectedOption3;
    private bool _selectedOption4;

    public WindowGame()
    {
        InitializeComponent();
        DataContext = this;
        _quizQuestions = new Questions();
        LoadQuizData(0);
    }

    // Properties with INotifyPropertyChanged
    public string QuestionNumberString
    {
        get => _questionNumberString;
        set
        {
            _questionNumberString = value;
            OnPropertyChanged();
        }
    }

    public string Question
    {
        get => _question;
        set
        {
            _question = value;
            OnPropertyChanged();
        }
    }

    public string Option1
    {
        get => _option1;
        set
        {
            _option1 = value;
            OnPropertyChanged();
        }
    }

    public string Option2
    {
        get => _option2;
        set
        {
            _option2 = value;
            OnPropertyChanged();
        }
    }

    public string Option3
    {
        get => _option3;
        set
        {
            _option3 = value;
            OnPropertyChanged();
        }
    }

    public string Option4
    {
        get => _option4;
        set
        {
            _option4 = value;
            OnPropertyChanged();
        }
    }

    public string ScoreString
    {
        get => _scoreString;
        set
        {
            _scoreString = value;
            OnPropertyChanged();
        }
    }

    public bool IsSubmitEnabled
    {
        get => _isSubmitEnabled;
        set
        {
            _isSubmitEnabled = value;
            OnPropertyChanged();
        }
    }


    public bool SelectedOption1
    {
        get => _selectedOption1;
        set
        {
            if (value)
            {
                _selectedOption1 = value;
                _selectedOption2 = false;
                _selectedOption3 = false;
                _selectedOption4 = false;
                IsSubmitEnabled = true;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SelectedOption2));
                OnPropertyChanged(nameof(SelectedOption3));
                OnPropertyChanged(nameof(SelectedOption4));
            }
        }
    }

    public bool SelectedOption2
    {
        get => _selectedOption2;
        set
        {
            if (value)
            {
                _selectedOption2 = value;
                _selectedOption1 = false;
                _selectedOption3 = false;
                _selectedOption4 = false;
                IsSubmitEnabled = true;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SelectedOption1));
                OnPropertyChanged(nameof(SelectedOption3));
                OnPropertyChanged(nameof(SelectedOption4));
            }
        }
    }

    public bool SelectedOption3
    {
        get => _selectedOption3;
        set
        {
            if (value)
            {
                _selectedOption3 = value;
                _selectedOption1 = false;
                _selectedOption2 = false;
                _selectedOption4 = false;
                IsSubmitEnabled = true;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SelectedOption1));
                OnPropertyChanged(nameof(SelectedOption2));
                OnPropertyChanged(nameof(SelectedOption4));
            }
        }
    }

    public bool SelectedOption4
    {
        get => _selectedOption4;
        set
        {
            if (value)
            {
                _selectedOption4 = value;
                _selectedOption1 = false;
                _selectedOption2 = false;
                _selectedOption3 = false;
                IsSubmitEnabled = true;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SelectedOption1));
                OnPropertyChanged(nameof(SelectedOption2));
                OnPropertyChanged(nameof(SelectedOption3));
            }
        }
    }

    public ICommand buttonPressed
    {
        get
        {
            if (_buttonPressed == null)
            {
                _buttonPressed = new RelayCommand(ExecuteButtonPressed, CanExecuteButtonPressed);
            }
            return _buttonPressed;
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void LoadQuizData(int index)
    {
        var question = _quizQuestions.GetQuestion(index);
        if (question != null)
        {
            QuestionNumberString = $"Question {index + 1} of {_quizQuestions.GetTotalQuestions()}";
            Question = question.QuestionText;
            Option1 = question.Options[0];
            Option2 = question.Options[1];
            Option3 = question.Options[2];
            Option4 = question.Options[3];
            ScoreString = _score.ToString();
            IsSubmitEnabled = false;

            // Clear radio button selections
            SelectedOption1 = false;
            SelectedOption2 = false;
            SelectedOption3 = false;
            SelectedOption4 = false;
        }
    }

    private void ExecuteButtonPressed(object parameter)
    {
        CheckAnswer();
    }

    private bool CanExecuteButtonPressed(object parameter)
    {
        return IsSubmitEnabled;
    }

    private void CheckAnswer()
    {
        // Get selected answer
        int selectedAnswer = -1;
        string selectedText = "";

        if (SelectedOption1)
        {
            selectedAnswer = 0;
            selectedText = Option1;
        }
        else if (SelectedOption2)
        {
            selectedAnswer = 1;
            selectedText = Option2;
        }
        else if (SelectedOption3)
        {
            selectedAnswer = 2;
            selectedText = Option3;
        }
        else if (SelectedOption4)
        {
            selectedAnswer = 3;
            selectedText = Option4;
        }

        if (selectedAnswer == -1)
        {
            MessageBox.Show("Please select an answer.", "No Selection", MessageBoxButton.OK);
            return;
        }

        // Get current question
        var currentQuestion = _quizQuestions.GetQuestion(_currentQuestionIndex);
        if (currentQuestion == null)
        {
            MessageBox.Show("Question not found.", "Error", MessageBoxButton.OK);
            return;
        }

        // Check answer
        bool isCorrect = selectedAnswer == currentQuestion.CorrectAnswer;

        if (isCorrect)
        {
            _score++;
            ScoreString = _score.ToString();
            MessageBox.Show($"✅ Correct! {currentQuestion.Explanation}", "Correct!", MessageBoxButton.OK);
        }
        else
        {
            MessageBox.Show($"❌ Incorrect!\n\nThe correct answer was: {currentQuestion.Options[currentQuestion.CorrectAnswer]}\n\n{currentQuestion.Explanation}", "Incorrect", MessageBoxButton.OK);
        }


        _currentQuestionIndex++;

        if (_currentQuestionIndex < _quizQuestions.GetTotalQuestions())
        {
            LoadQuizData(_currentQuestionIndex);
        }
        else
        {
            MessageBox.Show($"🎉 Quiz Completed!\n\nYour final score: {_score} out of {_quizQuestions.GetTotalQuestions()}", "Quiz Complete", MessageBoxButton.OK);
            IsSubmitEnabled = false;
        }
    }

    private void NextButton_Click(object sender, RoutedEventArgs e)
    {
        CheckAnswer();
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        MainWindow mainWindow = new MainWindow();
        mainWindow.Show();
    }
}

// RelayCommand implementation for ICommand
public class RelayCommand : ICommand
{
    private readonly Action<object> _execute;
    private readonly Func<object, bool> _canExecute;

    public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }

    public event EventHandler CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }

    public bool CanExecute(object parameter)
    {
        return _canExecute == null || _canExecute(parameter);
    }

    public void Execute(object parameter)
    {
        _execute(parameter);
    }
}