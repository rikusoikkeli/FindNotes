# FindNotes

## What does FindNotes do
FindNotes takes in arguments -query and -path. It searches -path for .txt files where -query appears somewhere within their contents. Then it prints the lines and some other information to the console. And offers the possibility to open said files straight from the command line.

## Background
Every day I take a lot of notes. I write down most of everything that I think about, talk about or hear in meetings. Writing things down is a way for me to make sense of the world.

Because of that, I quickly realised that I needed a way to search through all those .txt files in a swift manner. That is why I developed this command line application.

I didn't want to download a commercial product for the following reasons: Commercial products often use very specific file formats that bind the user to their application or to a specific operating system. They're also cluttered with features that most people don't need. And their graphical UIs are slow to use.

I mean, what happens if I want to send my notes to a different computer? What happens if the developers stop developing the app that I've learned to rely on? Or what happens if I realise that I need an extra feature?

I just want to open a Notepad and start instantly writing: *win+"notepad"+enter* 

And after, I want to be able to find that which I wrote quickly: *win+"cmd"+enter+"findnotes -q query -p path"*

Moreover, I want to get adjusted to a reliable workflow that I know will serve me for the following decade regardless of what machine I'm on. And regardless of what needs I will accrue.

And I really hate to use the mouse. Ain't got nobody time for that.

I'm happy about this program. It's right there in my top 10 most used programs. An inseparable part of my daily workflow.

## How to use
1) Add the program's executable into Windows path
2) Add some of your favourite project folders into the app's memory: *findnotes -save "C:\folder\subfolder" -nickname project1*
3) Now you can query the folder easily: *findnotes -query "data" -path project1*
4) FindNotes will paginate all the results  for "data" into the command line.
5) You can open a matching file by typing in the index number of the file (seen on the screen)

## Other
I do still develop this program every now and then. But I host the up-to-date version on a different service. So this one exists in GitHub only for demo purposes.
