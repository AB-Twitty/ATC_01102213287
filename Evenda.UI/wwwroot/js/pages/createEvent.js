$(document).ready(function () {
    $("#event-date").datepicker({
        startView: 1,
        todayBtn: "linked",
        keyboardNavigation: false,
        forceParse: false,
        autoclose: true,
        format: "mm/dd/yyyy",
        startDate: new Date()
    });

    VirtualSelect.init({
        ele: '.multiple-select',
        dropboxWrapper: 'self',
        optionsCount: 8,
        allowNewOption: true,
        optionsSelectedText: 'options selected',
        allOptionsSelectedText: 'All',
        showValueAsTags: true,
        noOfDisplayValues: 5,
        alwaysShowSelectedOptionsLabel: false,
        dropboxWidth: "100%",
        focusSelectedOptionOnOpen: true,
        showDropboxAsPopup: true,
        popupDropboxBreakpoint: '576px',
        popupPosition: 'center',
        hideValueTooltipOnSelectAll: true,
        maxWidth: '100%'
    });

    $(".summernote").summernote({
        callbacks: {
            onChange: function (contents, $editable) {
                $('#description-input').val(contents); 
            }
        }
    });

    $(".clockpicker").clockpicker({
        twelvehour: true, 
        autoclose: true
    });
});

$(document).ready(() => {
    // the total number of uploaded images
    var fileArray = [];

    // image upload
    $('#upload-btn').on('click', () => $('#image-input').click());

    // on uploading images, add them to the files array and view them 
    $('#image-input').on('change', function () {
        let files = this.files;
        let imageContainer = $('#image-gallery');

        for (let i = 0; i < files.length; i++) {
            let file = files[i];

            // some validation on the file type and size
            if (!file.type.startsWith('image/')) {
                alert('Only image files are allowed.');
                continue;
            }
            if (file.size > 5 * 1024 * 1024) {
                alert('File size exceeds 5MB');
                continue;
            }

            fileArray.push(file);

            (function (index) {
                let reader = new FileReader();
                reader.onload = (e) => {
                    // a new image will set as the thumbnail if no image is selected as thumbnail
                    let isThumbnail = $('input[name="ThumbnailKey"]:checked').length === 0;
                    let checked = isThumbnail ? 'checked' : '';

                    let imgHtml =
                        `
                    <div class="col-xl-3 col-lg-4 col-md-6 col-sm-12 d-flex flex-column image text-center p-2">
                        <div class="shadow d-flex flex-column flex-grow-1 border-2 border">
                            <img src="${e.target.result}" class="img-fluid pb-1" />
                            <button type="button" class="img-del btn btn-sm btn-danger" data-idx="new-${index}">
                                <i class="fa fa-close"></i>
                            </button>
                            <div class="d-flex mt-auto">
                                <div class="w-100 mb-1">
                                    <input ${checked} class="form-check-input" type="radio" name="ThumbnailKey" value="new-${index}" />
                                    <label class="m-0">Thumbnail</label>
                                </div>
                            </div>
                        </div>
                    </div>
                `;

                    imageContainer.append(imgHtml);
                };

                reader.readAsDataURL(file);
            })(fileArray.length - 1)
        }
    });

    // delete an image action
    $(document).on('click', '.img-del', function () {
        let wasThumbnail = $(this).closest('.image').find('input[name="ThumbnailKey"]').is(':checked');
        $(this).closest('.image').remove();

        // if the button has a data idx then update the file array
        var index = $(this).attr('data-idx');
        if (typeof index !== 'undefined' && index !== false && index !== null) {
            index = parseInt(index.split('-')[1], 10);
            if (!isNaN(index)) {
                fileArray.splice(index, 1);
                var dataTransfer = new DataTransfer();
                fileArray.forEach(function (file) {
                    dataTransfer.items.add(file);
                });
                $('#image-input')[0].files = dataTransfer.files;
            }
        }

        $('#image-gallery .img-del[data-idx]').each(function (i) {
            $(this).attr('data-idx', `new-${i}`);
        });

        // if the image to be removed is a thumbnail, then set the first image as the thumbnail
        if (wasThumbnail) {
            let firstImage = $('.image').first().find('input[name="ThumbnailKey"]');
            firstImage.prop('checked', true);
        }

        // if the image is not new then add the image id to the deletedImgsIds
        let key = $(this).attr('data-key');
        if (typeof key !== 'undefined' && key !== false && key !== null)
        {
            let deletedImgsIds = $('#DeletedImageIds').val();
            deletedImgsIds = deletedImgsIds ? deletedImgsIds.split(',') : [];
            deletedImgsIds.push(key);
            $('#DeletedImageIds').val(deletedImgsIds.join(','));
        }
    });
});