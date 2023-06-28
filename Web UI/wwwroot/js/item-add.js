//change this values if order of enums in DB/Enums will change (It shouldn't)
var WeaponEnum = '5';
var MiscellaneousEnum = '11';
var PotionEnum = '10';

$("#SelectType").on('change', function () {
    //we can't use hide and show, cause we use bootstrap d-none class to hide elements
    $("#WeaponDamage").addClass("d-none");
    $("#Modifiers").addClass("d-none");
    $("#Stats").removeClass("d-none"); //It's easier to show stats by default, cause only potion does not need stats

    var selectedType = $(this).val();
    console.log(selectedType);

    switch (selectedType) {
        case WeaponEnum:
            $("#WeaponDamage").removeClass("d-none");
            break;
        case MiscellaneousEnum:
            $("#Stats").addClass("d-none");
            break;
        case PotionEnum:
            $("#Modifiers").removeClass("d-none");
            $("#Stats").addClass("d-none");
            break;
        }
});

$(document).on('click', '.row-delete-btn', function () {
    $(this).closest(".modifier-row").remove();
});