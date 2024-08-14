using DialogGameConsole.Enums;
using DialogGameConsole.Options;
using DialogGameConsole.Text.Enums;
using DialogGameConsole.Texts;
using DialogGameConsole.Util;

namespace DialogGameConsole.DialogBase;

public class DialogEdge : DialogBase<DialogEdge>
{
    public EdgeType Type { get; }
    public DialogOption Option { get; init; }
    public DialogText Text { get; init; }
    public double Probability { get; init; }

    public bool IsTiming() => Type == EdgeType.Timing;

    public bool IsDirect() => Type == EdgeType.Direct;

    public bool IsOption() => Type == EdgeType.Option;

    public bool IsProbability() => Type == EdgeType.Probability;

    public static DialogEdge NewProbability(double probability)
    {
        Assert.IsWithin(probability, 0, 100, nameof(probability));
        return new DialogEdge(EdgeType.Probability)
        {
            Probability = probability
        };
    }

    public static DialogEdge NewDirect()
    {
        return new DialogEdge(EdgeType.Direct);
    }

    public static DialogEdge NewStop() {
        return new DialogEdge(EdgeType.Stop);
    }

    public static DialogEdge NewDelay(DialogText text)
    {
        Assert.IsNotNull(text, nameof(text));
        return new DialogEdge(EdgeType.Timing)
        {
            Text = text,
        };
    }

    public static DialogEdge NewOption(DialogOption option)
    {
        Assert.IsNotNull(option, nameof(option));
        return new DialogEdge(EdgeType.Option)
        {
            Option = option,
        };
    }

    private DialogEdge(EdgeType edgeType)
    {
        Type = edgeType;
    }

    public override string ToString()
    {
        var prefix = base.ToString() + " - " + Type.GetText();
        return Type switch
        {
            EdgeType.Timing => prefix + ", D: " + Text,
            EdgeType.Option => prefix + ", O: " + Option,
            EdgeType.Probability => prefix + ", P: " + Probability,
            _ => prefix
        };
    }

    public override bool Equals(object obj)
    {
        if (obj == null || obj.GetType() != typeof(DialogEdge))
            return false;
        return Id == ((DialogEdge)obj).Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

}
/*

function NewProbabilityEdge(probability_start, probability_end) {
return new DialogNetworkEdge(edge_type.probability, noone, message_source.none, noone, nb_type.none, probability_start, probability_end)
}

function DialogNetworkEdge(_type = edge_type.none, _delay = noone, _character = message_source.none, 
					    _option_text = noone, _option_type = nb_type.none, _probability_start = noone, _probability_end = noone) constructor {


function initialize(_type, _delay, _character, _option_text, _option_type,  _probability_start, _probability_end) {
	if(_type == edge_type.none) throw("Edge type can't be none")

	var delay_array = []
	var total_delay = 0
	if(_type == edge_type.timing) 
	{
		var no_source =  _character == message_source.none
		if(!is_array(_delay) && _delay <= 0) throw("Timing edge array need a delay bigger than 0");
		if(is_array(_delay) && len(_delay) == 0) throw("Timing edge array can't be empty");
		delay_array = is_array(_delay) ? _delay : (no_source ? [0, _delay] : [_delay])
		
		var typing_delay = 0
		var wait_delay = 0
		for(var i=0; i<len(delay_array); i++) {
			if(delay_array[i] < 0) throw("Delay can't be less than 0")		
			total_delay += delay_array[i]
			typing_delay += (i mod 2 == 0) ? delay_array[i] : 0
			wait_delay += (i mod 2 == 1) ? delay_array[i] : 0
		}
		if(total_delay <= 0) throw("Total delay can't be less or equal to 0")		
		if(typing_delay > 0 && no_source) throw("Wait with typing delay > 0 need a source other than none")	
	} 

	var option_index_from_type = -1
	if (_type == edge_type.option) 
	{
		if(string_length(_option_text) <= 0) throw("option text can't be empty")
		switch (_option_type) {
		    case nb_type.option_01:
		        option_index_from_type = 1
		        break;
			case nb_type.option_02:
				option_index_from_type = 2
		        break;
			case nb_type.option_03:
		        option_index_from_type = 3
		        break;
		    default:
		        throw("Invalid type for option: " + _type)
		}
	}	
	
	if(_type == edge_type.probability)
	{
		assert_number_between(_probability_start, 0, 1)		
		assert_number_between(_probability_end, 0, 1)
		if(_probability_start >= _probability_end) throw("probability start needs to be lower than probability end. Actual: Start: "+string(_probability_start) + " End: " + string(_probability_end))
		probability_start = _probability_start
		probability_end = _probability_end
	}	
	else
	{
		assert_noone(_probability_start)
		assert_noone(_probability_end)
	}
	
	type = _type
	delay_profile = delay_array
	delay = total_delay
	character = _character

	option_text = _option_text
	option_index = option_index_from_type
}
	

	
self.initialize(_type, _delay, _character, _option_text, _option_type, _probability_start, _probability_end)
}
}*/