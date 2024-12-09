using System;
using System.Collections.Generic;

// Класс User
public class User
{
    public string Name { get; }
    private ChatMediator _chatMediator;

    public User(string name, ChatMediator chatMediator)
    {
        Name = name;
        _chatMediator = chatMediator;
    }

    public void SendMessage(string message, string recipientName)
    {
        _chatMediator.SendMessage(message, this, recipientName);
    }

    public void ReceiveMessage(string message, string senderName)
    {
        Console.WriteLine($"{Name} получил сообщение от {senderName}: {message}");
    }

    public void ShowMessageHistory()
    {
        _chatMediator.ShowHistory(Name);
    }

    public void ExitChat()
    {
        _chatMediator.RemoveUser(this);
        Console.WriteLine($"{Name} вышел из чата.");
    }
}

// Класс посредник ChatMediator
public class ChatMediator
{
    private List<User> _users = new List<User>();
    private Dictionary<string, List<string>> _messageHistory = new Dictionary<string, List<string>>();

    public void AddUser(User user)
    {
        _users.Add(user);
        _messageHistory[user.Name] = new List<string>(); // Инициализация истории сообщений
    }

    public void RemoveUser(User user)
    {
        _users.Remove(user);
    }

    public void SendMessage(string message, User sender, string recipientName)
    {
        // Отправка сообщения только если получатель существует
        User recipient = _users.Find(u => u.Name == recipientName);
        if (recipient != null)
        {
            recipient.ReceiveMessage(message, sender.Name);
            _messageHistory[recipientName].Add($"Сообщение от {sender.Name}: {message}");
        }
        else
        {
            Console.WriteLine($"Получатель {recipientName} не найден.");
        }
    }

    public void ShowHistory(string userName)
    {
        if (_messageHistory.ContainsKey(userName))
        {
            Console.WriteLine($"\nИстория сообщений для {userName}:");
            foreach (var msg in _messageHistory[userName])
            {
                Console.WriteLine(msg);
            }
        }
    }
}

// Пример использования
class Program
{
    static void Main(string[] args)
    {
        ChatMediator chatMediator = new ChatMediator();

        User user1 = new User("Alice", chatMediator);
        User user2 = new User("Bob", chatMediator);
        User user3 = new User("Charlie", chatMediator);

        chatMediator.AddUser(user1);
        chatMediator.AddUser(user2);
        chatMediator.AddUser(user3);

        user1.SendMessage("Привет, Боб!", "Bob");
        user2.SendMessage("Привет, Алиса!", "Alice");
        user3.SendMessage("Привет всем!", "Alice"); // это сообщение не будет доставлено, т.к. нет конкретного получателя

        user1.ShowMessageHistory();
        user2.ShowMessageHistory();

        user1.ExitChat();
        user1.SendMessage("Как дела?", "Bob"); // Попытка отправить сообщение после выхода из чата
    }
}