
function getColumnNames() {
    // Sample column names
    var columnNames = ["Competition Name", "Location City", "Location State", "Location Country", "Entry Window Open", "Entry Window Close", "Final Judging Date", "Entry Fee"];
    return columnNames;
}
// Function to toggle the display of the popup menu
function togglePopupMenu() {
    var popupMenu = document.getElementById("popupMenu");
    if (popupMenu.style.display === "none" || popupMenu.style.display === "") {
        popupMenu.style.display = "block";
    } else {
        popupMenu.style.display = "none";
    }
}

// Function to populate the popup menu with column names
//function setupFilterMenu() {
//    var columnNames = getColumnNames();
//    columnNames.innerHTML = '';

//    columnNames.forEach(function (columnName) {
//        var listItem = document.createElement('li');
//        listItem.textContent = columnName;
//        listItem.onclick = function () {
//            alert("Clicked on " + columnName);
//        };
//        columnNames.appendChild(listItem);
//    });
//}

function populateFilterMenu(jsonObjects) {
    // Extract unique state values
    const states = jsonObjects.reduce((acc, obj) => {
        if (obj.LocationState && !acc.includes(obj.LocationState)) {
            acc.push(obj.LocationState);
        }
        return acc;
    }, []);
    const countries = jsonObjects.reduce((acc, obj) => {
        if (obj.LocationCountry && !acc.includes(obj.LocationCountry)) {
            acc.push(obj.LocationCountry);
        }
        return acc;
    }, []);

    // Get the container div
    const statesContainer = document.getElementById('filter_states_checkboxlist');

    // Create checkboxes
    states.forEach(state => {
        const checkbox = document.createElement('input');
        checkbox.type = 'checkbox';
        checkbox.name = 'state';
        checkbox.id = 'cb_' + state.replace(/\s/g, '');
        checkbox.value = state;

        const label = document.createElement('label');
        label.textContent = state;
        label.setAttribute('for', checkbox.id);

        statesContainer.appendChild(checkbox);
        statesContainer.appendChild(label);
    });

    const countriesContainer = document.getElementById('filter_countries_checkboxlist');

    // Create checkboxes
    countries.forEach(country => {
        const checkbox = document.createElement('input');
        checkbox.type = 'checkbox';
        checkbox.name = 'country';
        checkbox.id = 'cb_' + country.replace(/\s/g, '');
        checkbox.value = country;

        const label = document.createElement('label');
        label.textContent = country;
        label.setAttribute('for', checkbox.id);

        countriesContainer.appendChild(checkbox);
        countriesContainer.appendChild(label);
    });
}

// Function to populate dropdown menus with unique values based on JSON data
//function populateDropdowns(jsonData) {
//    //var cityDropdown = document.getElementById("cityDropdown");
//    //var stateDropdown = document.getElementById("stateDropdown");
//    //var countryDropdown = document.getElementById("countryDropdown");
//    //var entryWindowOpenDropdown = document.getElementById("entryWindowOpenDropdown");
//    //var entryWindowCloseDropdown = document.getElementById("entryWindowCloseDropdown");
//    //var finalJudgingDateDropdown = document.getElementById("finalJudgingDateDropdown");
//    //var entryFeeDropdown = document.getElementById("entryFeeDropdown");

//    var stateCheckboxDiv = document.getElementById("filter_states_checkboxlist");

//    //<input type="checkbox" id="cb_states_AL" />
//    //        <label for="cb_states_AL">Alabama</label>
//    //        <input type="checkbox" id="cb_states_VA" />
//    //        <label for="cb_states_VA">Virginia</label>
//    //        <input type="checkbox" id="cb_states_OK" />
//    //        <label for="cb_states_OK">Oklahoma</label>

//    //var cities = [];
//    var states = [];
//    var countries = [];
//    //var entryWindowOpens = [];
//    //var entryWindowCloses = [];
//    //var finalJudgingDates = [];
//    //var entryFees = [];

//    jsonData.forEach(function (competition) {
//        //if (!cities.includes(competition.LocationCity)) {
//        //    cities.push(competition.LocationCity);
//        //    var cityItem = document.createElement("li");
//        //    cityItem.textContent = competition.LocationCity;
//        //    cityDropdown.appendChild(cityItem);
//        //}
//        if (!states.includes(competition.LocationState)) {
//            states.push(competition.LocationState);
//            //var stateItem = document.createElement("li");
//            var stateItem = document.createElement("li");
//            stateItem.textContent = competition.LocationState;
//            stateDropdown.appendChild(stateItem);
//        }
//        if (!countries.includes(competition.LocationCountry)) {
//            countries.push(competition.LocationCountry);
//            var countryItem = document.createElement("li");
//            countryItem.textContent = competition.LocationCountry;
//            countryDropdown.appendChild(countryItem);
//        }
//        //if (!stateCountries.includes(competition.LocationState + ", " + competition.LocationCountry)) {
//        //    stateCountries.push(competition.LocationState + ", " + competition.LocationCountry);
//        //    var stateCountryItem = document.createElement("li");
//        //    stateCountryItem.textContent = competition.LocationState + ", " + competition.LocationCountry;
//        //    stateCountryDropdown.appendChild(stateCountryItem);
//        //}
//        //if (!entryWindowOpens.includes(competition.EntryWindowOpen)) {
//        //    entryWindowOpens.push(competition.EntryWindowOpen);
//        //    var entryWindowOpenItem = document.createElement("li");
//        //    entryWindowOpenItem.textContent = competition.EntryWindowOpen;
//        //    entryWindowOpenDropdown.appendChild(entryWindowOpenItem);
//        //}
//        //if (!entryWindowCloses.includes(competition.EntryWindowClose)) {
//        //    entryWindowCloses.push(competition.EntryWindowClose);
//        //    var entryWindowCloseItem = document.createElement("li");
//        //    entryWindowCloseItem.textContent = competition.EntryWindowClose;
//        //    entryWindowCloseDropdown.appendChild(entryWindowCloseItem);
//        //}
//        //if (!finalJudgingDates.includes(competition.FinalJudgingDate)) {
//        //    finalJudgingDates.push(competition.FinalJudgingDate);
//        //    var finalJudgingDateItem = document.createElement("li");
//        //    finalJudgingDateItem.textContent = competition.FinalJudgingDate;
//        //    finalJudgingDateDropdown.appendChild(finalJudgingDateItem);
//        //}
//        //if (!entryFees.includes(competition.EntryFee)) {
//        //    entryFees.push(competition.EntryFee);
//        //    var entryFeeItem = document.createElement("li");
//        //    entryFeeItem.textContent = competition.EntryFee;
//        //    entryFeeDropdown.appendChild(entryFeeItem);
//        //}
//    });
//}

function blankForNull(dataString) {
    if (dataString == null)
        return "";
    else if (dataString.toLowerCase() == "null")
        return "";
    else
        return dataString;
}

// Function to populate the table with JSON data
function populateTable(data) {
    var tableBody = document.getElementById('tableBody');
    tableBody.innerHTML = '';

    data.forEach(function (competition) {
        var row = document.createElement('tr');
        row.innerHTML = '<td>' + competition.CompetitionName + '</td>' +
            '<td>' + blankForNull(competition.LocationCity) + '</td>' +
            '<td>' + blankForNull(competition.LocationState) + '</td>' +
            '<td>' + blankForNull(competition.LocationCountry) + '</td>' +
            '<td>' + competition.EntryWindowOpen + '</td>' +
            '<td>' + competition.EntryWindowClose + '</td>' +
            '<td>' + competition.FinalJudgingDate + '</td>' +
            '<td>' + competition.EntryFee + '</td>';
        tableBody.appendChild(row);
    });
}

// Function to sort the table
function sortTable(columnIndex) {
        var table, rows, switching, i, x, y, shouldSwitch, dir, switchCount = 0;
table = document.getElementById("competitionTable");
switching = true;
// Set the sorting direction to ascending
dir = "asc";
while (switching) {
    switching = false;
rows = table.rows;
for (i = 1; i < (rows.length - 1); i++) {
    shouldSwitch = false;
x = rows[i].getElementsByTagName("td")[columnIndex];
y = rows[i + 1].getElementsByTagName("td")[columnIndex];
if (dir == "asc") {
                    if (x.innerHTML.toLowerCase() > y.innerHTML.toLowerCase()) {
    shouldSwitch = true;
break;
                    }
                } else if (dir == "desc") {
                    if (x.innerHTML.toLowerCase() < y.innerHTML.toLowerCase()) {
    shouldSwitch = true;
break;
                    }
                }
            }
if (shouldSwitch) {
    rows[i].parentNode.insertBefore(rows[i + 1], rows[i]);
switching = true;
switchCount++;
            } else {
                if (switchCount == 0 && dir == "asc") {
    dir = "desc";
switching = true;
                }
            }
        }
    }

    
