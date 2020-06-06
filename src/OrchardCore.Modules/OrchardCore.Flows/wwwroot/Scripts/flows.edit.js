/*
** NOTE: This file is generated by Gulp and should not be edited directly!
** Any changes made directly to this file will be overwritten next time its asset group is processed by Gulp.
*/

//variables used in FlowPart.Edit sortable
var widgetDragItem, lastContainer, widgetItemSourceId, widgetItemDestId;
$(function () {
  $(document).on('change', '.widget-editor-footer label, .widget-editor-header label', function () {
    var $tmpl = $(this).closest('.widget-template');
    var $radio = $(this).find("input:first-child");

    if ($radio[0].id !== 'undefined' && $radio[0].id.indexOf('Size') > 0) {
      var $radioSize = $(this).find("input:first-child").val();
      var classList = $tmpl.attr('class').split(' ');
      $.each(classList, function (id, item) {
        if (item.indexOf('col-md-') === 0) $tmpl.removeClass(item);
      });
      var colSize = Math.round($radioSize / 100 * 12);
      $tmpl.addClass('col-md-' + colSize);
      var dropdown = $(this).closest('.dropdown-menu');
      dropdown.prev('button').text($radioSize + '%');
    } else if ($radio[0].id !== 'undefined' && $radio[0].id.indexOf('Alignment') > 0) {
      var svg = $(this).find('svg')[0].outerHTML;
      var alignDropdown = $(this).closest('.dropdown-menu');
      var $btn = alignDropdown.prev('button');
      $btn.html(svg);
    }

    $(document).trigger('contentpreview:render');
  });
  $(document).on('keyup', '.widget-editor .form-group input[data-card-bind], .widget-editor', function (e) {
    // Do nothing if it's inline view
    if ($(e.target).parent().is(".inline-card-item")) {
      return;
    }

    var title = $(this).val();
    var widgetEditor = $(this).closest('.widget-editor');
    var data = $(this).data('card-bind'); //find bound element

    var boundElement = widgetEditor.find('[data-card-bind*=' + data + ']').filter(function () {
      return $(this).closest('.widget-editor').is(widgetEditor) && e.target != this;
    }); //trigger change event,to allow each bound element to configure display differently.

    boundElement.each(function () {
      $(this).trigger('contentcard:change', [data, title]);
    });
  });
  $(document).on('contentcard:change', 'span[data-card-bind], label[data-card-bind], div[data-card-bind]', function (evt, prop, val) {
    // default - change inner html with value
    $(this).html(val);
  });
  $(document).on('click', '.btn.toggleAll', function (e) {
    e.preventDefault();
    var that = $(this);
    var iscollapsed = that.find('svg').is('.fa-angle-double-down');

    if (iscollapsed) {
      $('.toggleAll > svg.fa-angle-double-down').toggleClass('fa-angle-double-down fa-angle-double-up');
      $('.widget-template .widget.widget-editor.card.collapsed').filter(':not(.widget-nocollapse)').removeClass('collapsed');
    } else {
      $('.toggleAll > svg.fa-angle-double-up').toggleClass('fa-angle-double-down fa-angle-double-up');
      $('.widget-template .widget.widget-editor.card').filter(':not(.widget-nocollapse):not(.collapsed)').addClass('collapsed');
    }
  });
});