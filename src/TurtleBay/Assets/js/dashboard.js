$(document).ready(function() 
{
    setInterval(function () 
    {
        $.ajax({ url: "/tb/api", dataType:'json' }).then(function(data) 
        {
           $('#now').html(data.Now);
           $('#hc_header').html(data.HeatingCounter);
           $('#lc_header').html(data.LightingCounter);
           $('#pc_header').html(data.ProgramCounter);
           $('#temperature_header').html(data.Temperature + " °C");
           $('#lighting_header').html(data.Lighting == "True" ? "An" : "Aus");
           $('#heating_header').html(data.Heating == "True" ? "An" : "Aus");

           $('#temperature').removeClass('bg-success');
           $('#temperature').removeClass('bg-warning');
           $('#temperature').removeClass('bg-danger');

           if (data.Temperature == "NaN")
           {
                $('#temperature').addClass('bg-danger');
		   }
           else if(parseInt(data.Temperature) < parseInt(data.Min))
           {
                $('#temperature').addClass('bg-warning');
           }
           else if (parseInt(data.Temperature) > parseInt(data.Max))
           {
                $('#temperature').addClass('bg-danger');
           }
           else
           {
                $('#temperature').addClass('bg-success');
           }

           $('#lighting').removeClass('bg-info');
           $('#lighting').removeClass('bg-success');

           if (data.Lighting == "True")
           {
                $('#lighting').addClass('bg-success');
		   }
           else
           {
                $('#lighting').addClass('bg-info');
		   }

           $('#heating').removeClass('bg-info');
           $('#heating').removeClass('bg-success');

           if (data.Heating == "True")
           {
                $('#heating').addClass('bg-success');
		   }
           else
           {
                $('#heating').addClass('bg-info');
		   }
        });
    }, 1000);
});