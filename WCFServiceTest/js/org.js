//node(<a/>) Toggle
function orgtoggle() {
    $('#tree ul a').click(function () {
        $(this).nextAll().slideToggle(200);

    });

    $('#tree a').click(function () {
        if ($(this).hasClass('') || $(this).hasClass('azure')) {
            $(this).removeClass('azure');
            $(this).addClass("red");
        }
        else if ($(this).hasClass('red')) {
            $(this).removeClass('red');
            $(this).toggleClass("azure");
        }
    });
}

//Build root node 
(function ($) {
    $.fn.first_Orgchart = function (e) {
        $(this).append(
            $('<ul/>')
                .append(
                $('<li/>').append(
                    $('<ul/>')
                        .attr('id', e)
                )
                )
        );
    };
})(jQuery);

//Build root node children
function second_Orgchart(e1, e2) {
    $('#' + e1).append(
        $('<li/>')
            .append(
            $('<ul/>')
                .attr('id', e2)
            )
    );
}

//Insert data into node
function Info_Insert(e1, e2) {
    $('#' + e1).parent().prepend(
        $('<a/>')
            .attr('href', 'javascript:;')
    );
    $('#' + e1).parent().prepend(
        $('<div/>')
            .text(e2)
    );

}

//show number= 01,02,...,09,10
function NumFormat(e) {
    var num;
    if (e < 10 && e >= 0) {
        num = '0' + e;
    } else if (e > 10) {
        num = e;
    } else {
        alert('function NumFormat has some problem!!');
    }
    return num;
}

//OrgChart's dataTable


//Do OrgChart_DataTable's data be repeated? 
function OrgChart_DataTable_CheckRepeater(e) {
    var table = e;
    var tableRowCount = table.length;
    var repeatarray = [];
    var repeatCount = 0;



    for (var i = 0; i < tableRowCount; i++) {
        var str = table[i]['id'];
        var boolcount = 0;

        for (var j = 0; j < tableRowCount; j++) {
            if (str === table[j]['id']) {
                boolcount++;
            }
            if (boolcount === 2) {
                repeatarray[repeatCount] = table[j]['id'];
                repeatCount++;
                break;
            }
        }
    }

    if (repeatCount > 0) {
        var ShowRepeater = repeatarray.join();
        alert('Your dataID[{ ' + ShowRepeater + ' }] has already repeated!!');
        return false;
    } else {
        return true;
    }

}

//Do OrgChart_DataTable's data conform format?
function OrgChart_DataTable_CheckFormat(e) {
    var table = e;
    var tableRowCount = table.length;
    var str = [];
    var ErrorCount = 0;
    var ShowError = [];
    var count = 0;

    for (var i = 0; i < tableRowCount; i++) {
        str[i] = table[i]['id'].split('-');
        for (var j = 0; j < str[i].length; j++) {
            if (str[i][j].length !== 2) {
                ShowError[ErrorCount] = table[i]['id'];
                ErrorCount++;
            }
        }
    }

    if (ErrorCount > 0) {
        var strError = ShowError.join();
        alert("Your DataID [{ " + strError + " }] don't conform the format!!");
        return false;
    } else {
        return true;
    }

}

//BuildOrgChart   
(function ($) {
    $.fn.BuildOrgChart = function (e) {
        var table = e;
        var tableRowCount = table.length;
        var count = 0;
        var numarray = [];
        var getInfo = [];
        var str_remove = '';

        //Declare 2's dimension array
        for (i = 0; i < 10; i++) {
            numarray[i] = 0;
            getInfo[i] = [];
        }

        //Check DataTable hierarchy
        for (i = 0; i < tableRowCount; i++) {
            var tablecount = table[i]['id'].split('-').length;

            if (tablecount >= count) {
                count = tablecount;
            }

            //OrgChart has 10 hierarchy
            switch (tablecount) {
                case 1:
                    getInfo[0][numarray[0]] = table[i]['id'];
                    numarray[0]++;
                    break;
                case 2:
                    getInfo[1][numarray[1]] = table[i]['id'];
                    numarray[1]++;
                    break;
                case 3:
                    getInfo[2][numarray[2]] = table[i]['id'];
                    numarray[2]++;
                    break;
                case 4:
                    getInfo[3][numarray[3]] = table[i]['id'];
                    numarray[3]++;
                    break;
                case 5:
                    getInfo[4][numarray[4]] = table[i]['id'];
                    numarray[4]++;
                    break;
                case 6:
                    getInfo[5][numarray[5]] = table[i]['id'];
                    numarray[5]++;
                    break;
                case 7:
                    getInfo[6][numarray[6]] = table[i]['id'];
                    numarray[6]++;
                    break;
                case 8:
                    getInfo[7][numarray[7]] = table[i]['id'];
                    numarray[7]++;
                    break;
                case 9:
                    getInfo[8][numarray[8]] = table[i]['id'];
                    numarray[8]++;
                    break;
                case 10:
                    getInfo[9][numarray[9]] = table[i]['id'];
                    numarray[9]++;
                    break;
                default:
                    alert('Your hierarchy is more than 10!!');
            }
        }

        //Build root node
        for (var i = 0; i < getInfo[0].length; i++) {
            $(this).first_Orgchart(getInfo[0][i]);
        }

        //Build root node children
        for (i = 1; i < 10; i++) {
            if (getInfo[i].length > 0) {
                for (var j = 0; j < getInfo[i].length; j++) {
                    var array = getInfo[i][j].split('-');
                    var remove = array.pop();
                    second_Orgchart(array.join('-'), getInfo[i][j]);
                }
            }
        }

        //Insert data into node
        for (i = 0; i < table.length; i++) {
            Info_Insert(table[i]['id'], table[i]['remark']);
        }

        //Remove overage element
        str_remove = $('ul:not(:has(*))');
        str_remove.remove();
    };
})(jQuery);

//create a method call EzOrgChart
(function ($) {
    $.fn.EzOrgChart = function (e) {

        if (OrgChart_DataTable_CheckFormat(e) && OrgChart_DataTable_CheckRepeater(e)) {

            $(this).BuildOrgChart(e);
            $('#tree a').addClass('azure');
            orgtoggle();

        }
    };
})(jQuery);

//----------------------------------------------Refesh Table---------------------------------------------
//sort Two-dimension smaller -> bigger
function SortByName(a, b) {
    var aName = a.id.toLowerCase();
    var bName = b.id.toLowerCase();
    return ((aName < bName) ? -1 : ((aName > bName) ? 1 : 0));
}

//Produce parent node
var Ezrecursion = function () {

}

$.extend(Ezrecursion.prototype, {
    //Create a newtable
    init_recursion: function (e) {
        var table = [];
        var strArray = [];
        var bool;
        var booltable;
        var count = 0;

        for (i = e.length - 1; i >= 0; i--) {
            bool = false;
            strArray[i] = e[i]['id'].split('-');
            strArray[i].pop();
            for (j = i - 1; j >= 0; j--) {
                bool = (strArray[i].join('-') !== e[j]['id']) ? true : false;
                if (!bool)
                    break;
            }

            if (bool) {
                if (table.length === 0) {
                    table.push({ id: strArray[i].join('-') });
                } else {
                    booltable = true;
                    for (k = 0; k < table.length; k++) {
                        booltable = (strArray[i].join('-') === table[k]['id']) ? false : true;
                    }
                    if (booltable && strArray[i].join('-') !== '') {
                        table.push({ id: strArray[i].join('-') });
                    }
                }
            }
        }

        for (i = 0; i < table.length; i++) {
            e.push(table[i]);
        }

        return table;
    },
    //Recursion newtable
    recursion: function (e) {
        var check = true;
        return function (table) {

            if (!check) {
                return table;
            } else {

                var newtable = [];
                var strArray = [];
                var bool;
                var booltable;

                for (i = 0; i < table.length; i++) {
                    strArray[i] = table[i]['id'].split('-');
                    strArray[i].pop();

                    for (j = 0; j < e.length; j++) {
                        bool = (strArray[i].join('-') === e[j]['id']) ? false : true;
                        if (!bool) {
                            break;
                        }
                    }

                    if (bool) {
                        if (newtable.length === 0 && strArray[i].join('-') !== '') {
                            newtable.push({ id: strArray[i].join('-') });
                        } else {
                            booltable = true;
                            for (k = 0; k < newtable.length; k++) {
                                booltable = (strArray[i].join('-') === newtable[k]['id']) ? false : true;
                            }
                            if (booltable && strArray[i].join('-') !== '') {
                                newtable.push({ id: strArray[i].join('-') });
                            }
                        }
                    }
                }

                if (newtable.length === 0) {
                    check = false;
                }
                else {
                    for (i = 0; i < newtable.length; i++) {
                        e.push(newtable[i]);
                    }
                }
                return arguments.callee(newtable);
            }
        };
    }
});

//Refesh table and return
function SortAndCreateNodeIntoTable(e) {
    var rec = new Ezrecursion();
    var table = rec.init_recursion(e);
    rec.recursion(e)(table);
    e.sort(SortByName);
    return e;
}

