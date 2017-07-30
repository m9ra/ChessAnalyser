var ACTIVE_BOARDS = {};

var on_board_updated = function (board_name, history) {
    send_command('on_board_updated', {
        board_name: board_name,
        history: history
    });
};

var handle_commands = function (data) {
    for (var command_index in data) {
        var command = data[command_index];
        var command_name = command["name"];
        switch (command_name) {
            case 'set_board_position':
                var board_name = command["board_name"];
                var board = ACTIVE_BOARDS[board_name];
                board.position(command["current_fen"]);
                board.available_moves = command["available_moves"];
                break;

            case 'set_board_navigation':
                var board_name = command["board_name"];
                var info = command["info"];
                $("#" + board_name + "_navigation").html(info);
                break;

            default:
                alert("Unknown command " + command_name + ". " + command);
                break;
        }
    }
}

var send_command = function (command_name, command_data) {
    command_data["name"] = command_name;
    var commands = [command_data];
    var commands_json = JSON.stringify(commands);

    $.post("/ajax_commands", { commands_json: commands_json })
        .done(handle_commands);
}