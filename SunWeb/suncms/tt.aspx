<%@ Page Language="C#" Inherits="PageView<MainModel>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>SunCMS Administrator Panel</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
    <link href="style/default/suncms-base.css" rel="stylesheet" />
    <link href="style/default/suncms-style.css" rel="stylesheet" />
    <script type="text/javascript" src="javascript/lib/jquery-last.min.js"></script>
    <script type="text/javascript" src="javascript/lib/jquery-form.js"></script>
    <script type="text/javascript" src="javascript/lib/underscore-last.min.js"></script>
    <script type="text/javascript" src="javascript/lib/knockout-last.js"></script>
    <script type="text/javascript" src="javascript/lib/knockout.mapping-latest.debug.js"></script>

</head>

<body>
    <p>Send me spam: <input type="checkbox" data-bind="checked: wantsSpam" /></p>
    <div data-bind="visible: wantsSpam">
        Preferred flavor of spam:
        <div><input type="radio" name="flavorGroup" value="cherry" data-bind="checked: spamFlavor" /> Cherry</div>
        <div><input type="radio" name="flavorGroup" value="almond" data-bind="checked: spamFlavor" /> Almond</div>
        <div><input type="radio" name="flavorGroup" value="msg" data-bind="checked: spamFlavor" /> Monosodium Glutamate</div>
    </div>
     
    <script type="text/javascript">
         viewModel = {
            wantsSpam: ko.observable(true),
            spamFlavor: ko.observable("almond") // Initially selects only the Almond radio button
        };
         
        // ... then later ...
        //viewModel.spamFlavor("msg"); // Now only Monosodium Glutamate is checked

        ko.applyBindings(viewModel);
    </script>
</body>
</html>

