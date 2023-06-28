function AddPartialContent($content, $TargetjQuerySelector) {
    let Target = $($TargetjQuerySelector);

    $.get($content, function (data) {
        Target.append(data);
    });
}