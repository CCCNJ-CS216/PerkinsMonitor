var formsActivated = false;

function tryLookup(id)
{
	if(id.length == 6 && (!formsActivated))
	{
		formsActivated = true;

		var tObj = $.ajax({
		url: "/SignIn/GetStudent", 

		data: {
			studentID: id
		},

		type: "POST",

		dataType: "json",
		}).done(function(json){return json.responseJSON;});

		$('<div class="label">Name:</div><input type="text" id="name" name="name"><div id="name_tt" class="tooltip">We require that you put in both your first and last name</div><br><div class="label">Major:</div><input type="text" id="major" name="major"><br><div class="label">Machine Number</div><input type="text" name="machineNumber" id="machineNumber">').appendTo($("#fields"));

		$.when(tObj).done(function(){
			$("#name").val(tObj.responseJSON["name"]);
			$("#major").val(tObj.responseJSON["major"]);
		});
	}
}

function signIn_validation(){

	if(!formsActivated)
		return;

	if( $("#name").val().includes(" "))
	{
		$("#name_tt").hide("slow");
	}
	else
	{
		$("#name_tt").show("slow");
	}
}

function signIn_setup(){
	$(document).ready(function(){
		setInterval(signIn_validation, 1000);});
}