////Create a newtable to recursion
//function OrgCharts_recursion(e) {
//    var table = [];
//    var strArray = [];
//    var bool;
//    var booltable;
//    var count = 0;

//    for (i = e.length - 1; i >= 0; i--) {
//        bool = false;
//        strArray[i] = e[i]['id'].split('-');
//        strArray[i].pop();
//        for (j = i - 1; j >= 0; j--) {
//            bool = (strArray[i].join('-') !== e[j]['id']) ? true : false;
//            if (!bool)
//                break;
//        }

//        if (bool) {
//            if (table.length === 0) {
//                table.push({ id: strArray[i].join('-') });
//            } else {
//                booltable = true;
//                for (k = 0; k < table.length; k++) {
//                    booltable = (strArray[i].join('-') === table[k]['id']) ? false : true;
//                }
//                if (booltable && strArray[i].join('-') != '') {
//                    table.push({ id: strArray[i].join('-') });
//                }
//            }
//        }
//    }

//    for (i = 0; i < table.length; i++) {
//        e.push(table[i]);
//    }

//    return table;
//}

////Recursion newtable
//function recursion(e) {
//    var check = true;
//    return function (table) {

//        if (!check) {
//            return table;
//        } else {

//            var newtable = [];
//            var strArray = [];
//            var bool;
//            var booltable;

//            for (i = 0; i < table.length; i++) {
//                strArray[i] = table[i]['id'].split('-');
//                strArray[i].pop();

//                for (j = 0; j < e.length; j++) {
//                    bool = (strArray[i].join('-') === e[j]['id']) ? false : true;
//                    if (!bool) {
//                        break;
//                    }
//                }

//                if (bool) {
//                    if (newtable.length === 0 && strArray[i].join('-') != '') {
//                        newtable.push({ id: strArray[i].join('-') });
//                    } else {
//                        booltable = true;
//                        for (k = 0; k < newtable.length; k++) {
//                            booltable = (strArray[i].join('-') === newtable[k]['id']) ? false : true;
//                        }
//                        if (booltable && strArray[i].join('-') != '') {
//                            newtable.push({ id: strArray[i].join('-') });
//                        }
//                    }
//                }
//            }

//            if (newtable.length === 0) {
//                check = false;
//            }
//            else {
//                for (i = 0; i < newtable.length; i++) {
//                    e.push(newtable[i]);
//                }
//            }
//            return arguments.callee(newtable);
//        }
//    };
//}

var recursion = function () {
    
}

$.extend(recursion.prototype, {
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
                    if (booltable && strArray[i].join('-') != '') {
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
                        if (newtable.length === 0 && strArray[i].join('-') != '') {
                            newtable.push({ id: strArray[i].join('-') });
                        } else {
                            booltable = true;
                            for (k = 0; k < newtable.length; k++) {
                                booltable = (strArray[i].join('-') === newtable[k]['id']) ? false : true;
                            }
                            if (booltable && strArray[i].join('-') != '') {
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