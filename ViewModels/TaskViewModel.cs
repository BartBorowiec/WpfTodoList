using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TaskManager.Data;
using TaskManager.Models;

namespace TaskManager.ViewModels
{
    class TaskViewModel : INotifyPropertyChanged
    {
        public ICommand SaveTaskCommand { get; set; }
        public ICommand DeleteTaskCommand { get; set; }
        public ICommand DeselectTaskCommand { get; set; }
        public ICommand MarkAsDoneCommand { get; set; }

        public ICommand SaveNoteCommand { get; set; }
        public ICommand DeleteNoteCommand { get; set; }
        public ICommand DeselectNoteCommand { get; set; }

        private bool? _allIsChecked = true;
        public bool? AllIsChecked
        {
            get
            {
                return _allIsChecked;
            }
            set
            {
                _allIsChecked = value;
                if (_allIsChecked == true)
                {
                    PastIsChecked = true;
                    IncomingIsChecked = true;
                    Tasks = context.Tasks.ToList();
                }
                if(_allIsChecked == false)
                {
                    AllIsChecked = true;
                }
                UpdateTaskList();
                NotifyPropertyChanged("AllIsChecked");
            }
        }
        private bool _pastIsChecked = true;
        public bool PastIsChecked
        {
            get
            {
                return _pastIsChecked;
            }
            set
            {
                _pastIsChecked = value;
                UpdateAllCheckbox();
                UpdateTaskList();
                NotifyPropertyChanged("PastIsChecked");
            }
        }

        private bool _incomingIsChecked = true;
        public bool IncomingIsChecked
        {
            get
            {
                return _incomingIsChecked;
            }
            set
            {
                _incomingIsChecked = value;
                UpdateAllCheckbox();
                UpdateTaskList();
                NotifyPropertyChanged("IncomingIsChecked");
            }
        }

        private void UpdateAllCheckbox()
        {
            if (PastIsChecked && IncomingIsChecked)
            {
                _allIsChecked = true;
            }
            else if (!PastIsChecked && !IncomingIsChecked)
            {
                _allIsChecked = false;
            }
            else
            {
                _allIsChecked = null;
            }
            NotifyPropertyChanged("AllIsChecked");
        }
        private void UpdateTaskList()
        {
            if (PastIsChecked && IncomingIsChecked)
            {
                Tasks = context.Tasks.ToList();
            }
            else if (!PastIsChecked && !IncomingIsChecked)
            {
                Tasks.Clear();
            }
            else if(PastIsChecked) 
            {
                Tasks = context.Tasks.Where(t => t.Deadline < DateTime.Now).ToList();
            }
            else
            {
                Tasks = context.Tasks.Where(t => t.Deadline >= DateTime.Now).ToList();
            }
            NotifyPropertyChanged("Tasks");
        }



        private MyTask task = new MyTask();//Obiekt powiązany z formularzem
        private MyNote note = new MyNote();
        
        public MyTask _selectedTask;
        public MyTask SelectedTask
        {
            get
            {
                return _selectedTask;
            }
            set
            {
                _selectedTask = value;
                if(_selectedTask != null)
                {
                    TaskContent = SelectedTask.Content;
                    Deadline = SelectedTask.Deadline;
                    Priority = SelectedTask.Priority;
                }
                NotifyPropertyChanged("SelectedTask");
            }
        }

        private List<MyTask> _tasks;
        public List<MyTask> Tasks
        {
            get
            {
                return _tasks;
            }
            set
            {
                _tasks = value;
                NotifyPropertyChanged("Tasks");
            }
        }

        private string _searchPhrase = "";
        public string SearchPhrase
        {
            get
            {
                return _searchPhrase;
            }
            set
            {
                _searchPhrase = value;
                NotifyPropertyChanged("SearchPhrase");
                Tasks = context.Tasks
                    .Where(t => t.Content.ToLower().Contains(_searchPhrase.ToLower()))
                    .ToList();
            }
        }

        public string TaskContent
        {
            get
            {
                return task.Content;
            }
            set
            {
                task.Content = value;
                Debug.WriteLine($"TaskContent {TaskContent}");
                NotifyPropertyChanged("TaskContent");
            }
        }

        public DateTime Deadline
        {
            get
            {
                return task.Deadline;
            }
            set
            {
                task.Deadline = value;
                NotifyPropertyChanged("Deadline");
            }
        }

        public int Priority
        {
            get
            {
                return task.Priority;
            }
            set
            {
                task.Priority = value;
                NotifyPropertyChanged("Priority");
            }
        }

        //public bool IsCompleted
        //{
        //    get
        //    {
        //        return task.IsCompleted;
        //    }
        //    set
        //    {
        //        task.IsCompleted = value;
        //        NotifyPropertyChanged("IsCompleted");
        //    }
        //}

        public MyNote _selectedNote;
        public MyNote SelectedNote
        {
            get
            {
                return _selectedNote;
            }
            set
            {
                _selectedNote = value;
                NotifyPropertyChanged("SelectedNote");
            }
        }
        private List<MyNote> _notes;
        public List<MyNote> Notes
        {
            get
            {
                return _notes;
            }
            set
            {
                _notes = value;
                NotifyPropertyChanged("Notes");
            }
        }


        public string NoteContent
        {
            get
            {
                return note.Content;
            }
            set
            {
                note.Content = value;
                NotifyPropertyChanged("NoteContent");
            }
        }

        private TaskDbContext context;

        public TaskViewModel(TaskDbContext context)
        {
            this.context = context;
            task.Deadline = DateTime.Now;
            note.Created = DateTime.Now;
            Tasks = context.Tasks.ToList();
            Notes = context.Notes.ToList();
            SaveTaskCommand = new MyCommand(SaveTask, IsTaskFormNotEmpty);
            DeleteTaskCommand = new MyCommand(DeleteTask, IsTaskSelected);
            DeselectTaskCommand = new MyCommand(DeselectTask, IsTaskSelected);
            MarkAsDoneCommand = new MyCommand(MarkAsDone, IsTaskSelected);
            SaveNoteCommand = new MyCommand(InsertNote, IsNoteFormNotEmpty);
            DeleteNoteCommand = new MyCommand(DeleteNote, IsNoteSelected);
            DeselectNoteCommand = new MyCommand(DeselectNote, IsNoteSelected);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                Debug.WriteLine($"Property {propertyName} changed");
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void SaveTask(object obj)
        {
            if(SelectedTask  != null)
            {
                var modifiedTask = context.Tasks.First(t => t.Id == SelectedTask.Id);
                modifiedTask.Content = TaskContent;
                modifiedTask.Deadline = Deadline;
                modifiedTask.Priority = Priority;   
            }
            else
            {
                task.IsCompleted = false;
                context.Tasks.Add(task);
            }
            context.SaveChanges();
            task = new MyTask();
            Tasks = context.Tasks
                .Where(t => t.Content.ToLower().Contains(_searchPhrase.ToLower()))
                .ToList();
        }

        private bool IsTaskFormNotEmpty(object obj)
        {
            return string.IsNullOrEmpty(task.Content) == false;
        }

        private void DeselectTask(object obj)
        {
            SelectedTask = null;
        }

        private bool IsTaskSelected(object obj)
        {
            return SelectedTask != null;
        }

        public void DeleteTask(object obj)
        {
            if (SelectedTask == null) return;
            if (MessageBox.Show("Potwierdzasz usunięcie tego rekordu?", "Zadania", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    context.Tasks.Remove(context.Tasks.Find(SelectedTask.Id));
                    context.SaveChanges();
                    Tasks = context.Tasks.ToList();
                    MessageBox.Show("Rekord został usunięty.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Błąd w czasie zapisu. " + ex.InnerException);
                }
            }
        }

        public void InsertNote(object obj)
        {
            note.Created = DateTime.Now;
            context.Notes.Add(note);
            context.SaveChanges();
            note = new MyNote();
            Notes = context.Notes.ToList();
        }

        private bool IsNoteFormNotEmpty(object obj)
        {
            return string.IsNullOrEmpty(note.Content) == false;
        }

        private void DeselectNote(object obj)
        {
            SelectedNote = null;
        }

        private bool IsNoteSelected(object obj)
        {
            return SelectedNote != null;
        }

        public void DeleteNote(object obj)
        {
            if (SelectedNote == null) return;
            if (MessageBox.Show("Potwierdzasz usunięcie tego rekordu?", "Notatki", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    context.Notes.Remove(context.Notes.Find(SelectedNote.Id));
                    context.SaveChanges();
                    Notes = context.Notes.ToList();
                    MessageBox.Show("Rekord został usunięty.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Błąd w czasie zapisu. " + ex.InnerException);
                }
            }
        }

        public void OnCheckBoxCheckedChanged(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;

            var taskToUpdate = context.Tasks.First(t => t.Id == SelectedTask.Id);
            taskToUpdate.IsCompleted = checkBox.IsChecked ?? false;
            context.SaveChanges();
            Tasks = context.Tasks.ToList();
        }

        public void MarkAsDone(object obj)
        {
            var taskToUpdate = context.Tasks.First(t => t.Id == SelectedTask.Id);
            taskToUpdate.IsCompleted = true;
            context.SaveChanges();
            Tasks = context.Tasks.ToList();
        }
    }
}
