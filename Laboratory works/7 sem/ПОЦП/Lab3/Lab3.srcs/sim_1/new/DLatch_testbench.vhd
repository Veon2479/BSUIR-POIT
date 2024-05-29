----------------------------------------------------------------------------------
-- Company: 
-- Engineer: 
-- 
-- Create Date: 11/05/2023 07:13:40 PM
-- Design Name: 
-- Module Name: DLatch_testbench - Behavioral
-- Project Name: 
-- Target Devices: 
-- Tool Versions: 
-- Description: 
-- 
-- Dependencies: 
-- 
-- Revision:
-- Revision 0.01 - File Created
-- Additional Comments:
-- 
----------------------------------------------------------------------------------


library IEEE;
use IEEE.STD_LOGIC_1164.ALL;

-- Uncomment the following library declaration if using
-- arithmetic functions with Signed or Unsigned values
--use IEEE.NUMERIC_STD.ALL;

-- Uncomment the following library declaration if instantiating
-- any Xilinx leaf cells in this code.
--library UNISIM;
--use UNISIM.VComponents.all;

entity DLatch_testbench is
--  Port ( );
end DLatch_testbench;

architecture Behavioral of DLatch_testbench is
    component DLatch is
    Port ( D : in STD_LOGIC;
           Q : out STD_LOGIC;
           NQ : out STD_LOGIC);
    end component;

    component DLatch_beh is 
    Port ( D : in STD_LOGIC;
           Q : out STD_LOGIC;
           NQ : out STD_LOGIC);
    end component;
    
    function to_stdlogic(i: integer) return std_logic is
    begin
        case i is
            when 0 => return '0';
            when 1 => return '1';
            when others => return 'X';
        end case;
    end function;
    
    signal d, qb, nqb, qs, nqs : std_logic;
begin
    s: Dlatch port map(D=>d, Q=>qb, NQ=>nqb);
    b : DLatch_beh port map (D=>d, Q=>qs, NQ=>nqs);
    
    Main: process
    begin
        for i in 0 to 1 loop 
                d <= to_stdlogic(i);
                wait for 10 ns;
                assert ((qb /= qs) or (nqb /= nqs)) report "Testbench failed!";
                wait for 10 ns;
        end loop;
        report "Testbench finished";
        wait;
    end process;

end Behavioral;
