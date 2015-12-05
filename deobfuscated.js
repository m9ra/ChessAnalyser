(function() {
    zf9bd6 = function() {
        this.table_to_code = {"a1": "a","a2": "i","a3": "q","a4": "y","a5": "G","a6": "O","a7": "W","a8": "4","b1": "b","b2": "j","b3": "r","b4": "z","b5": "H","b6": "P","b7": "X","b8": "5","c1": "c","c2": "k","c3": "s","c4": "A","c5": "I","c6": "Q","c7": "Y","c8": "6","d1": "d","d2": "l","d3": "t","d4": "B","d5": "J","d6": "R","d7": "Z","d8": "7","e1": "e","e2": "m","e3": "u","e4": "C","e5": "K","e6": "S","e7": "0","e8": "8","f1": "f","f2": "n","f3": "v","f4": "D","f5": "L","f6": "T","f7": "1","f8": "9","g1": "g","g2": "o","g3": "w","g4": "E","g5": "M","g6": "U","g7": "2","g8": "!","h1": "h","h2": "p","h3": "x","h4": "F","h5": "N","h6": "V","h7": "3","h8": "?"};
        this.table_from_code = {"_a": "a1","_i": "a2","_q": "a3","_y": "a4","_G": "a5","_O": "a6","_W": "a7","_4": "a8","_b": "b1","_j": "b2","_r": "b3","_z": "b4","_H": "b5","_P": "b6","_X": "b7","_5": "b8","_c": "c1","_k": "c2","_s": "c3","_A": "c4","_I": "c5","_Q": "c6","_Y": "c7","_6": "c8","_d": "d1","_l": "d2","_t": "d3","_B": "d4","_J": "d5","_R": "d6","_Z": "d7","_7": "d8","_e": "e1","_m": "e2","_u": "e3","_C": "e4","_K": "e5","_S": "e6","_0": "e7","_8": "e8","_f": "f1","_n": "f2","_v": "f3","_D": "f4","_L": "f5","_T": "f6","_1": "f7","_9": "f8","_g": "g1","_o": "g2","_w": "g3","_E": "g4","_M": "g5","_U": "g6","_2": "g7","_!": "g8","_h": "h1","_p": "h2","_x": "h3","_F": "h4","_N": "h5","_V": "h6","_3": "h7","_?": "h8"};
        this.prom_q1 = "{";
        this.prom_q2 = "~";
        this.prom_q3 = "}";
        this.prom_n1 = "(";
        this.prom_n2 = "^";
        this.prom_n3 = ")";
        this.prom_r1 = "[";
        this.prom_r2 = "_";
        this.prom_r3 = "]";
        this.prom_b1 = "@";
        this.prom_b2 = "#";
        this.prom_b3 = "$";
    };
    zf9bd6.prototype = {move_to_code: function(source_sq_tr, target_sq_tr, promoted_piece) {
            var source_sq = this.table_to_code[source_sq_tr];
            var target_sq = this.table_to_code[target_sq_tr];
            if (promoted_piece) {
                var dir = target_sq_tr.charCodeAt(0) - source_sq_tr.charCodeAt(0);
                if (promoted_piece == "q")
                    target_sq = (dir == -1) ? this.prom_q1 : ((dir == +1) ? this.prom_q3 : this.prom_q2);
                else 
                if (promoted_piece == "n")
                    target_sq = (dir == -1) ? this.prom_n1 : ((dir == +1) ? this.prom_n3 : this.prom_n2);
                else 
                if (promoted_piece == "r")
                    target_sq = (dir == -1) ? this.prom_r1 : ((dir == +1) ? this.prom_r3 : this.prom_r2);
                else 
                if (promoted_piece == "b")
                    target_sq = (dir == -1) ? this.prom_b1 : ((dir == +1) ? this.prom_b3 : this.prom_b2);
            }
            return source_sq + target_sq;
        },code_to_move: function(code) {
            var source_sq = code.charAt(0);
            var target_sq = code.charAt(1);
            var source_sq_tr = this.table_from_code["_" + source_sq];
            var target_sq_tr;
            var promoted_piece = null;
            var source_sq_tr_file = source_sq_tr.charCodeAt(0);
            var promotion_file_tr = null;
            if (target_sq == this.prom_q1) {
                promotion_file_tr = source_sq_tr_file - 1;
                promoted_piece = "q";
            } else 
            if (target_sq == this.prom_q2) {
                promotion_file_tr = source_sq_tr_file;
                promoted_piece = "q";
            } else 
            if (target_sq == this.prom_q3) {
                promotion_file_tr = source_sq_tr_file + 1;
                promoted_piece = "q";
            } else 
            if (target_sq == this.prom_n1) {
                promotion_file_tr = source_sq_tr_file - 1;
                promoted_piece = "n";
            } else 
            if (target_sq == this.prom_n2) {
                promotion_file_tr = source_sq_tr_file;
                promoted_piece = "n";
            } else 
            if (target_sq == this.prom_n3) {
                promotion_file_tr = source_sq_tr_file + 1;
                promoted_piece = "n";
            } else 
            if (target_sq == this.prom_r1) {
                promotion_file_tr = source_sq_tr_file - 1;
                promoted_piece = "r";
            } else 
            if (target_sq == this.prom_r2) {
                promotion_file_tr = source_sq_tr_file;
                promoted_piece = "r";
            } else 
            if (target_sq == this.prom_r3) {
                promotion_file_tr = source_sq_tr_file + 1;
                promoted_piece = "r";
            } else 
            if (target_sq == this.prom_b1) {
                promotion_file_tr = source_sq_tr_file - 1;
                promoted_piece = "b";
            } else 
            if (target_sq == this.prom_b2) {
                promotion_file_tr = source_sq_tr_file;
                promoted_piece = "b";
            } else 
            if (target_sq == this.prom_b3) {
                promotion_file_tr = source_sq_tr_file + 1;
                promoted_piece = "b";
            }
            if (promotion_file_tr != null) {
                var pre_promotion_row_tr = source_sq_tr.charAt(1);
                if (pre_promotion_row_tr == 7)
                    target_sq_tr = String.fromCharCode(promotion_file_tr) + "8";
                else 
                if (pre_promotion_row_tr == 2)
                    target_sq_tr = String.fromCharCode(promotion_file_tr) + "1";
                else
                    return null;
            } else {
                target_sq_tr = this.table_from_code["_" + target_sq];
                if (!target_sq_tr)
                    return null;
            }
            var result = {};
            result["fromArea"] = source_sq_tr;
            result["toArea"] = target_sq_tr;
            result["additionalInfo"] = promoted_piece;
            return result;
        },za902f: function(zcc2ef) {
            return this.table_to_code[zcc2ef];
        },z398d0: function(zccfb6) {
            return this.table_from_code["_" + zccfb6];
        }};
})();