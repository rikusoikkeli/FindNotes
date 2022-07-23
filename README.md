# FindNotes

## Background
Every day I take a lot of notes. I write down most of everything that I think about, talk about or hear in meetings. Writing things down is a way for me to make sense of the world.

Because of that, I quickly realised that I needed a way to search through all those .txt files in a swift manner. That is why I developed this command line application.

I didn't want to download a commercial product for the following reasons: Commercial products often use very specific file formats that bind oneself to their application or the operating system one is using. They're also cluttered with features that most people don't need. Moreover, what happens if I want to send my notes to a different computer? What happens if the developers stop developing the app that I've learned to rely on? Or what happens if I realise that I need an extra feature?

I just want to open a Notepad (win+"note"+enter) and start instantly writing. And after, I want to be able to find that which I wrote quickly and easily. And I want to get adjusted to a reliable workflow that I know will serve me for the following decade regardless of what machine I'm on. And regardless of what needs I will develop.

I'm really happy about this program. It's right there in my top 10 most used programs. An inseparable part of my daily workflow.

## How to use
1) Add the program's executable into Windows path
2) Add some of your favourite project folders into the app's memory: findnotes -save "C:\folder\subfolder" -nickname project1
3) Now you can query the folder easily: findnotes -query "data" -path project1
4) FindNotes will paginate all the results  for "data" into the command line
5) You can open a matching file by typing in the index number of the file (seen on the screen)

## Other
I do still develop this program every now and then. But I host the up-to-date version in a different service. So this one exists in GitHub only for demo purposes.