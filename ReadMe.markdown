## XNA Presentation Framework

A layout framework for [XNA](http://create.msdn.com/) on Windows & Windows Phone 7 from [Red Badger Consulting](http://red-badger.com/blog/tag/xpf/). Inspiration is drawn from the [WPF](http://msdn.microsoft.com/en-us/library/ms754130.aspx) and [Silverlight](http://www.microsoft.com/silverlight/) XAML stacks, [Reactive Extensions](http://msdn.microsoft.com/en-us/data/gg577609.aspx) is used to keep things nice and fast.

### Getting started

**Getting started with Git and GitHub**

 * People new to GitHub should consider using [GitHub for Windows](http://windows.github.com/).
 * If you decide not to use GHFW you will need to:
  1. [Set up Git and connect to GitHub](http://help.github.com/win-set-up-git/)
  2. [Fork the XPF repository](http://help.github.com/fork-a-repo/)
 * Finally you should look into [git - the simple guide](http://rogerdudler.github.com/git-guide/)

Once you're familiar with Git and GitHub, clone the repository and run the ```.\build.cmd``` script to compile the code and run all the unit tests. You can use this script to test your changes quickly.

**Rules for Our Git Repository**

 * We use ["A successful Git branching model"](http://nvie.com/posts/a-successful-git-branching-model/). What this means is that:
   * You need to branch off of the [develop branch](https://github.com/redbadger/XPF/tree/develop) when creating new features or non-critical bug fixes.
   * Each logical unit of work must come from a single and unique branch:
     * A logical unit of work could be a set of related bugs or a feature.
     * You should wait for us to accept the pull request (or you can cancel it) before commiting to that branch again.
 * Your code must attain a complete pass StyleCop before submitting a pull request.
 * You must write a [MSpec](https://github.com/machine/machine.specifications) specification for your code with at least 90% coverage. One of the following tools can make this simpler:
   * [NCrunch](http://www.ncrunch.net/) - a commercial product that is currently free. *Some tests spontaneously fail, possibly due to threading subtleties, re-running the tests a few times usually shows you exactly what is passing. Alternatively confirm the results using the MSpec executable.*
   * [ReSharper](http://www.jetbrains.com/resharper/) - a commercial product that can be extended with MSpec functionality.
   * The MSpec executable in the repository.
   * The ```.\build.cmd``` will also run MSpec after a build. The resulting HTML file will be placed into the output Bin directory.
 * A license header on each source file.

Apart from the branching model and code coverage, running the ```.\build.cmd``` with 'zero-red' means you can do a pull request.

### Getting Involved

We will be using pull requests to vet potential contributors; please do not ask for push rights - we will approach you if we feel that you can be trusted with them.

### License

XPF uses the MIT license, which can be found in license.markdown. The gist of the MIT license is that you can use XPF in your closed-source projects, without any encumberances from us. You can freely compile this code and distribute it to customers (whether they are on XBox, Windows, Windows Phone, Linux or Mac) without having to publish your source code anywhere.

**Additional Restrictions**

 * We only accept code that is compatible with the MIT license (essentially, BSD and Public Domain).
 * Copying copy-left (GPL-style) code is strictly forbidden.
 * The MIT license must be prepended to each source code file as a code comment.
 * You are not allowed to copy dissasembled Microsoft WPF or Silverlight code (closed-source/proprietary) into the repository.
 * You are not allowed to copy original Mono Moonlight code (LGPL) into the repository.