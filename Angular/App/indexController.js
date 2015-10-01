(function() {
    angular.module('car-finder').controller('indexController',
    ['$interval', '$timeout', function($interval, $timeout) {
        var self = this;
        this.value = 1;

        $interval(function() {
            self.time = new Date();
        }, 100);

        this.fname = "thomas";
        this.lname = "parrish";

        this.currentfname = 0;
        this.currentlname = 0;

        this.prevName = function() {
            if (self.currentfname > 0) {
                self.currentfname--;
                self.currentlname--;
            } else {
                self.currentfname = self.fnames.length - 1;
                self.currentlname = self.lnames.length - 1;
            }
        }



        this.fnames = ["thomas", "bob", "john", "sue", "steve"];
        this.lnames = ["parrish", "james", "davis", "cotton", "bush"];

        this.frand = function() {
            return Math.floor(Math.random() * self.fnames.length);
        }

        this.lrand = function() {
            return Math.floor(Math.random() * self.lnames.length);
        }

        this.isPreviousRowFilled = function (currentRow) {
            if (currentRow == 0) return true;
            var guesses = self.rows[currentRow-1].guesses;
            for(var i = 0; i < guesses.length; i++)
                if (guesses[i] == null || guesses[i] == '')
                    return false;
            return true;
        }

        this.rows = [
            {
                guesses: ['','','','','']
            },
            {
                guesses: ['', '', '', '', '']
            },
            {
                guesses: ['', '', '', '', '']
            },
            {
                guesses: ['', '', '', '', '']
            },
            {
                guesses: ['', '', '', '', '']
            },
            {
                guesses: ['', '', '', '', '']
            },
            {
                guesses: ['', '', '', '', '']
            },
            {
                guesses: ['', '', '', '', '']
            },
            {
                guesses: ['', '', '', '', '']
            },
            {
                guesses: ['', '', '', '', '']
            }
        ];


    }]);
})();