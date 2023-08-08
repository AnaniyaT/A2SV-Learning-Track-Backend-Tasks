using Blog.Data;
using Blog.Models;
using Blog.Services;

namespace Blog;

public class Cli
{
    private PostManager _postManager;
    private CommentManager _commentManager;
    private BlogDisplay _blogDisplay;

    public Cli(BlogDbContext context)
    {
        _postManager = new PostManager(context);
        _commentManager = new CommentManager(context);
        _blogDisplay = new BlogDisplay(context);
    }
    
    public string GetValidatedInput(string message, Func<string, string?> validator)
    {
        Console.WriteLine(message);
        string? input = Console.ReadLine();

        while (input == null || validator(input) != null)
        {
            Console.WriteLine(input == null ? "You must input something. Try again." : validator(input));
            input = Console.ReadLine();
        }

        return input;
    }
    
    void AddPostFromInput()
    {
        string title = GetValidatedInput(
            "Enter post title",
            name => name.Length > 2 ? null : "Title longer than 2 characters"
        );

        string content = GetValidatedInput(
            "Enter post content",
            input => null
        );

        _postManager.Create(new Post() { Title = title, Content = content});
        Console.WriteLine("Post created successfully");
    }
    
    void AddCommentFromInput()
    {
        int postId=0;
        string postNumber = GetValidatedInput(
            "Enter post number",
            input => int.TryParse(input, out postId) ? null : "Title longer than 2 characters"
        );

        string content = GetValidatedInput(
            "Enter comment text",
            input => null
        );

        if (_postManager.GetById(postId) is null)
        {
            Console.WriteLine("Post not found");
            return;
        }

        _commentManager.Create(new Comment() { PostId = postId, Text = content });
        Console.WriteLine("Comment added Successfully");
    }
    
    void PrintCommandList()
    {
        Console.WriteLine(""" 
        List of Commands:
            ls      :  List All Posts.
            details :  View details of a post. Type details followed by post number. eg. 'details 2'
            add     :  Add a post.
            comment :  Add a comment to a post.
            del     :  Delete a post. Type del followed by comment number. eg. 'del 1'.
            del c   :  Delete a comment. Type del c followed by comment number. eg. 'del c 1'.
            h       :  Help.
            q       : quit.
        """);
    }

    void DeleteComment(string[] command)
    {
        int commentId;
        if (int.TryParse(command[2], out commentId))
        {
            if (_commentManager.GetById(commentId) is null)
            {
                Console.WriteLine("Comment does not exist");
                return;
            }

            _commentManager.Delete(commentId);
            Console.WriteLine("Comment deleted");
        }
    }

    void DeletePost(string[] command)
    {
        int postId;
        if (int.TryParse(command[1], out postId))
        {
            if (_postManager.GetById(postId) is null)
            {
                Console.WriteLine("Post not found");
                return;
            }
            
            _postManager.Delete(postId);
            Console.WriteLine("Post Deleted Successfully");
            return;
        }
        Console.WriteLine("Invalid post number");
        
    }

    void CommandHandler(string[] command)
    {
        switch (command[0].ToLower())
        {
            case ("ls"):
                _blogDisplay.DisplayPosts();
                break;
            case("add"):
                AddPostFromInput();
                break;
            case("h"):
                PrintCommandList();
                break;
            case ("del"):
                if (command.Length < 2)
                {
                    Console.WriteLine("Delete arguments needed");
                    break;
                }
                
                if (command.Length > 2)
                {
                    if (command[1].ToLower() != "c")
                    {
                        Console.WriteLine("Invalid command");
                        break;
                    }
                    DeleteComment(command);
                    break;
                }
                DeletePost(command);
                break;
            
            case("details"):
                if (command.Length < 2)
                {
                    Console.WriteLine("Post number needed");
                    break;
                }
                int postId;
                if (!int.TryParse(command[1], out postId) )
                {
                    Console.WriteLine("Invalid position");
                }

                _blogDisplay.DisplayDetails(postId);
                break;
            case("comment"):
                AddCommentFromInput();
                break;
            default:
                Console.WriteLine("Invalid command");
                break;
        }
        
    }
    
    public void Start()
    {
        Console.WriteLine("Welcome To Blog App.");
        
        PrintCommandList();

        bool running = true;
        while (running)
        {
            String inputCommand = GetValidatedInput(
                "\nInput Command (ls, add, del, details, h, q)",
                input => input.Length >= 1 ? null : "Invalid command"
            );

            String[] splitInput = inputCommand.Split();
            if (splitInput[0].ToLower() == "q")
            {
                running = false;
            }
            else
            {
                CommandHandler(splitInput);
            }
        }
    }
}