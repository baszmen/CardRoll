using CardRoll.Control.Board;
using GalaSoft.MvvmLight;

namespace CardRoll.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private Board board;
        private Board jokerBoard;
        private Board newBoard;

        public Board Board
        {
            get
            {
                return this.board;
            }
            set
            {
                this.RaisePropertyChanged("Board");
            }
        }

        public Board JokerBoard
        {
            get
            {
                return this.jokerBoard;
            }
            set
            {
                this.RaisePropertyChanged("JokerBoard");
            }
        }

        public Board NewBoard
        {
            get
            {
                return this.newBoard;
            }
            set
            {
                this.RaisePropertyChanged("NewBoard");
            }
        }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            this.board = new Board(4,4);
            this.jokerBoard = new Board(1, 2);
            this.newBoard = new Board(1, 4);

            this.Board = this.board;
            this.JokerBoard = this.jokerBoard;
            this.NewBoard = this.newBoard;
        }
    }
}