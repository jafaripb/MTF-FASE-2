
var vendor =  new Bloodhound({
        datumTokenizer: function (d) { return Bloodhound.tokenizers.whitespace(d.text); },
        queryTokenizer: Bloodhound.tokenizers.whitespace,
        //limit:Number.MAX_VALUE, // I do not know how to get total number ...
        remote: {
            url: 'data/vendor.json'
            }
        });
 
vendor.initialize();

/**
 * Categorizing tags
 */
elt = $('#vendor');
elt.tagsinput({
  
  itemValue: 'value',
  itemText: 'text',
  // typeaheadjs: {
  //   name: 'cities',
  //   displayKey: 'text',
  //   source: cities.ttAdapter()
  // }
  typeaheadjs: [
  {
      hint: true,
     highlight: true,
     minLength: 2
  },
   {
      name: 'vendor',
       displayKey: 'text',
       source: vendor.ttAdapter(),
	   templates: {
			suggestion: Handlebars.compile(
				'<div class="vendor">'+									
					'<div class="box-typehead-content">'+                
					  '<span class="box-typehead-title">{{text}}</span>'+
					  '<span class="box-typehead-desk">{{kualifikasi}},{{distibutor}}</span>'+
					  '<span class="box-typehead-desk">{{kontak}}</span>'+
					'</div>'+				  
				'</div>'
			)}	
   },
   
  ]
});

// HACK: overrule hardcoded display inline-block of typeahead.js
$(".twitter-typeahead").css('display', 'inline');




var person =  new Bloodhound({
        datumTokenizer: function (d) { return Bloodhound.tokenizers.whitespace(d.text); },
        queryTokenizer: Bloodhound.tokenizers.whitespace,
        //limit:Number.MAX_VALUE, // I do not know how to get total number ...
        remote: {
            url: 'data/person.json'
            }
        });
 
person.initialize();

/**
 * Categorizing tags
 */
$('#person').tagsinput({
  
  itemValue: 'value',
  itemText: 'text',
  typeaheadjs: [
  {
      hint: true,
     highlight: true,
     minLength: 2
  },
   {
      name: 'person',
       displayKey: 'text',
       source: person.ttAdapter(),
	   templates: {
			suggestion: Handlebars.compile(
				'<div class="person">'+
					'<div class="box-typehead">'+ 
					'<img class="box-typehead-icon" src="{{logo}}">'+					
					'<div class="box-typehead-content">'+                
					  '<span class="box-typehead-title">{{text}}</span>'+
					  '<span class="box-typehead-desk">{{divisi}}</span>'+
					'</div>'+
				  '</div>'+
				'</div>'
			)}	
   },
   
  ]
});

$('#person2').tagsinput({
  
  itemValue: 'value',
  itemText: 'text',
  typeaheadjs: [
  {
      hint: true,
     highlight: true,
     minLength: 2
  },
   {
      name: 'person',
       displayKey: 'text',
       source: person.ttAdapter(),
	   templates: {
			suggestion: Handlebars.compile(
				'<div class="person">'+
					'<div class="box-typehead">'+ 
					'<img class="box-typehead-icon" src="{{logo}}">'+					
					'<div class="box-typehead-content">'+                
					  '<span class="box-typehead-title">{{text}}</span>'+
					  '<span class="box-typehead-desk">{{divisi}}</span>'+
					'</div>'+
				  '</div>'+
				'</div>'
			)}	
   },
   
  ]
});

$('#person2').tagsinput('add', { "value": 1 , "text": "Fajar Gunawa"});
$('#person2').tagsinput('add', { "value": 7 , "text": "Sukamto" });
