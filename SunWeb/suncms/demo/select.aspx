<%@ Page Language="C#" Inherits="PageView<MainModel>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>SunCMS Administrator Panel</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
    <script type="text/javascript" src="../javascript/lib/jquery-last.min.js"></script>
    <script type="text/javascript" src="../javascript/lib/jquery-form.js"></script>
    <script type="text/javascript" src="../javascript/lib/underscore-last.min.js"></script>
    <script type="text/javascript" src="../javascript/lib/knockout-last.js"></script>
    <script type="text/javascript" src="../javascript/lib/knockout.mapping-latest.debug.js"></script>

</head>

<body>
    <h2>Your seat reservations</h2>

    <table>
        <thead><tr>
            <th>Passenger name</th><th>Meal</th><th>Surcharge</th><th></th>
        </tr></thead>
        <tbody data-bind="foreach: seats">
            <tr>
                <td><input data-bind="value: name" /></td>
                <td><select data-bind="options: $root.availableMeals, value: meal, optionsText: 'mealName'"></select></td>
                <td data-bind="text: formattedPrice"></td>
                <td><a href="#" data-bind="click: $root.removeSeat">Remove</a></td>
            </tr>
        </tbody>
    </table>

    <button data-bind="click: addSeat">Reserve another seat</button>

    <h3 data-bind="visible: totalSurcharge() > 0">
        Total surcharge: $<span data-bind="text: totalSurcharge().toFixed(2)"></span>
    </h3>

    <script type="text/javascript">
        // Class to represent a row in the seat reservations grid
        function SeatReservation(name, initialMeal) {
            var self = this;
            self.name = name;
            self.meal = ko.observable(initialMeal);

            self.formattedPrice = ko.computed(function() {
                var price = self.meal().price;
                return price ? "$" + price.toFixed(2) : "None";        
            });    
        }

        // Overall viewmodel for this screen, along with initial state
        function ReservationsViewModel() {
            var self = this;

            // Non-editable catalog data - would come from the server
            self.availableMeals = [
                { mealName: "Standard (sandwich)", price: 0 },
                { mealName: "Premium (lobster)", price: 34.95 },
                { mealName: "Ultimate (whole zebra)", price: 290 }
            ];    

            // Editable data
            self.seats = ko.observableArray([
                new SeatReservation("Steve", self.availableMeals[0]),
                new SeatReservation("Bert", self.availableMeals[0])
            ]);

            // Computed data
            self.totalSurcharge = ko.computed(function() {
               var total = 0;
               for (var i = 0; i < self.seats().length; i++)
                   total += self.seats()[i].meal().price;
               return total;
            });    

            // Operations
            self.addSeat = function() {
                self.seats.push(new SeatReservation("", self.availableMeals[0]));
            }
            self.removeSeat = function(seat) { self.seats.remove(seat) }
        }

        ko.applyBindings(new ReservationsViewModel());

    </script>
</body>
</html>

