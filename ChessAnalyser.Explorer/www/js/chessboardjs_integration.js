var chessboardjs_init = function (board_name) {
    var board_history = [];


    document.writeln("<div id='" + board_name + "'/>");
    var boardEl = $('#' + board_name);

    var onDragStart = function (source, piece, position, orientation) {
        if (!(source in board.available_moves))
            return false;

        var moves = board.available_moves[source];

        boardEl.find('.square-' + source).addClass('move_highlight');
        for (var targetIndex in moves) {
            var target = moves[targetIndex];
            boardEl.find('.square-' + target).addClass('move_highlight');
        }
    };

    var onDrop = function (source, target) {
        boardEl.find('.move_highlight').removeClass('move_highlight');

        if (source == target)
            return false;

        if (!(source in board.available_moves))
            return false;

        if (board.available_moves[source].indexOf(target) >= 0)
            board_history.push([source, target]);
    };

    var onSnapEnd = function () {
        on_board_updated(board_name, board_history);
    };

    var cfg = {
        draggable: true,
        onDragStart: onDragStart,
        onDrop: onDrop,
        onSnapEnd: onSnapEnd
    };
    var board = ChessBoard(board_name, cfg);
    board.history = board_history;
    board.available_moves = {};

    ACTIVE_BOARDS[board_name] = board;
    on_board_updated(board_name, board_history);
};
