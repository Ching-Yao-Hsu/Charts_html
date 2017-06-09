    //node(<a/>) Toggle
    function orgtoggle(e) {
        $('a').on('click', function () {
            $(this).nextAll().toggle();
        });
    }

    //Build root node
    function first_Orgchart(e) {
        $('#tree').append(
            $('<ul/>')
            .append(
                $('<li/>').append(
                    $('<ul/>')
                    .attr('id', e)
                )
            )
        );
    }

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
    function Info_Insert(e) {
        $('#' + e).parent().prepend(
            $('<a/>')
            .attr('href', '#')
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
    function OrgChart_DataTable() {
        var table = [{
            id: '00',
            name: ''
            }, {
            id: '00-00',
            name: ''
            }, {
            id: '00-01',
            name: ''
            }, {
            id: '00-02',
            name: ''
            }, {
            id: '01',
            name: ''
            }, {
            id: '00-03',
            name: ''
            }, {
            id: '01-00',
            name: ''
            }, {
            id: '01-02',
            name: ''
            }, {
            id: '03',
            name: ''
            }, {
            id: '03-03',
            name: ''
            }];

        return table;
    }

    //Do OrgChart_DataTable's data be repeated? 
    function OrgChart_DataTable_CheckRepeater() {
        var table = OrgChart_DataTable();
        var tableRowCount = table.length;
        var boolcount = 0;
        var repeatarray = [];
        var repeatCount = 0;

        for (var i = 0; i < tableRowCount; i++) {
            var str = table[i]['id'];
            var boolcount = 0;

            for (var j = 0; j < tableRowCount; j++) {
                if (str == table[j]['id']) {
                    boolcount++;
                }
                if (boolcount == 2) {
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
    function OrgChart_DataTable_CheckFormat() {
        var table = OrgChart_DataTable();
        var tableRowCount = table.length;
        var str = [];
        var ErrorCount = 0;
        var ShowError = [];
        var count = 0;

        for (var i = 0; i < tableRowCount; i++) {
            str[i] = table[i]['id'].split('-');
            for (var j = 0; j < str[i].length; j++) {
                if (str[i][j].length != 2) {
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
    function BuildOrgChart() {
        var table = OrgChart_DataTable();
        var tableRowCount = table.length;
        var count = 0;
        var numarray = [];
        var getInfo = [];
        var str_remove = '';

        //Declare 2's dimension array
        for (var i = 0; i < 10; i++) {
            numarray[i] = 0;
            getInfo[i] = [];
        }

        //Check DataTable hierarchy
        for (var i = 0; i < tableRowCount; i++) {
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
            first_Orgchart(getInfo[0][i]);
        }

        //Build root node children
        for (var i = 1; i < 10; i++) {
            if (getInfo[i].length > 0) {
                for (var j = 0; j < getInfo[i].length; j++) {
                    var array = getInfo[i][j].split('-');
                    var remove = array.pop();
                    second_Orgchart(array.join('-'), getInfo[i][j]);
                }
            }
        }

        //Insert data into node
        for (var i = 0; i < table.length; i++) {
            Info_Insert(table[i]['id']);
        }

        //Remove overage element
        str_remove = $('ul:not(:has(*))');
        str_remove.remove();
    }
    
    function EzOrgChart(){
        if (OrgChart_DataTable_CheckFormat() && OrgChart_DataTable_CheckRepeater()) {
            BuildOrgChart();
            orgtoggle();
        } 
    }    
    
