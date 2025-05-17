$(function () {
    var selectedCountry = $('#selected-country').val();
    var selectedCity = $('#selected-city').val();

    let skipCityReset = 3;
    if (selectedCountry) skipCityReset = 0;

    VirtualSelect.init({
        ele: '#country-select',
        dropboxWrapper: 'self',
        dropboxWidth: "100%",
        focusSelectedOptionOnOpen: true,
        showDropboxAsPopup: true,
        popupDropboxBreakpoint: '576px',
        popupPosition: 'center',
        maxWidth: '100%',
        search: true,
        searchByStartsWith: true,
        hideClearButton: true,
        labelRenderer: countryFlagRenderer,
        selectedLabelRenderer: countryFlagRenderer
    });

    function countryFlagRenderer(data) {
        let countryCode = data.label.replace(/\s+/g, '-').toLowerCase();
        let prefix = `<i class="flag flag-${countryCode}"></i>`;
        return `${prefix}${data.label}`;
    }

    VirtualSelect.init({
        ele: '#city-select',
        dropboxWrapper: 'self',
        dropboxWidth: "100%",
        focusSelectedOptionOnOpen: true,
        showDropboxAsPopup: true,
        popupDropboxBreakpoint: '576px',
        popupPosition: 'center',
        maxWidth: '100%',
        search: true,
        searchByStartsWith: true,
        hideClearButton: true,
    });

    $.ajax({
        url: "https://api.countrystatecity.in/v1/countries",
        method: "GET",
        headers: {
            "X-CSCAPI-KEY": "amtFc3ptQlo5OUhsV2ZPSFBKM3dFRms2N1RQR3kzMFpETTByRmIzOQ=="
        },
        success: function (response) {
            if (!response.error) {
                var countryOpts = response.map(function (country) {
                    return { label: country.name, value: country.name, customData: country.iso2 };
                });

                document.querySelector('#country-select').setOptions(countryOpts);

                if (selectedCountry) {
                    skipCityReset = 1;
                    document.querySelector('#country-select').setValue(selectedCountry);

                    // Find iso2 for selected country
                    var selectedCountryObj = response.find(function (c) { return c.name === selectedCountry; });
                    if (selectedCountryObj) {
                        var iso2 = selectedCountryObj.iso2;
                        // Fetch and set city
                        $.ajax({
                            url: `https://api.countrystatecity.in/v1/countries/${iso2}/cities`,
                            type: 'GET',
                            headers: {
                                "X-CSCAPI-KEY": "amtFc3ptQlo5OUhsV2ZPSFBKM3dFRms2N1RQR3kzMFpETTByRmIzOQ=="
                            },
                            success: function (cityResponse) {
                                if (!cityResponse.error) {
                                    var cityOpts = cityResponse.map(function (city) {
                                        var name = city.name.normalize('NFD').replace(/[\u0300-\u036f]/g, '');
                                        return { label: name, value: name };
                                    });
                                    var citySelect = document.querySelector('#city-select');
                                    citySelect.setOptions(cityOpts);

                                    if (selectedCity) {
                                        citySelect.setValue(selectedCity);
                                    }
                                }
                            }
                        });
                    }
                }
            }
        },
        error: function (xhr, status, error) {
            console.log('An error occurred:', error);
        }
    });


    $('#country-select').on('change', function () {
        if (skipCityReset < 3) {
            skipCityReset++;
            return;
        }

        var country = document.querySelector('#country-select').getSelectedOptions();
        var iso2 = country ? country.customData : undefined;
        var citySelect = document.querySelector('#city-select');

        citySelect.reset();

        if (!iso2) {
            citySelect.disable();
            return;
        }

        citySelect.enable();

        $.ajax({
            url: `https://api.countrystatecity.in/v1/countries/${iso2}/cities`,
            type: 'GET',
            headers: {
                "X-CSCAPI-KEY": "amtFc3ptQlo5OUhsV2ZPSFBKM3dFRms2N1RQR3kzMFpETTByRmIzOQ=="
            },
            success: function (response) {
                if (!response.error) {
                    var cityOpts = response.map(function (city) {
                        var name = city.name.normalize('NFD').replace(/[\u0300-\u036f]/g, '');
                        return { label: name, value: name };
                    });
                    citySelect.setOptions(cityOpts);
                } else {
                    console.log('Error:', response.msg);
                }
            },
            error: function (xhr, status, error) {
                console.log('An error occurred:', error);
            }
        });
    });
});