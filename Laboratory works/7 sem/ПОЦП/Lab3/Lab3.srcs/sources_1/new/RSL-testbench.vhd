----------------------------------------------------------------------------------
-- Company: 
-- Engineer: 
-- 
-- Create Date: 11/04/2023 12:23:11 AM
-- Design Name: 
-- Module Name: RSL-testbench - Behavioral
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

entity RSL_testbench is
--  Port ( );
end RSL_testbench;

architecture Behavioral of RSL_testbench is
    Component RSLatch is 
        Port ( R : in STD_LOGIC;
           S : in STD_LOGIC;
           Q : out STD_LOGIC;
           NQ : out STD_LOGIC);
    end component;
    
    Component RSLatch_beh is 
        Port ( R : in STD_LOGIC;
           S : in STD_LOGIC;
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
    
    signal sq, snq, bq, bnq: std_logic;
    signal r : std_logic := '1';
    signal s : std_logic := '0';
begin
    str: RSLatch port map(R=>r, S=>s, Q=>sq, NQ=>snq);
    beh: RSLatch_beh port map(R=>r, S=>s, Q=>bq, NQ=>bnq);
    
    Main: process
    begin
        wait for 10 ns;
        for i in 0 to 1 loop 
            for j in 0 to 1 loop 
                r <= to_stdlogic(i);
                s <= to_stdlogic(j);
                wait for 10 ns;
                assert ((sq /= bq) or (snq /= bnq)) report "Testbench failed!";
                wait for 10 ns;
            end loop;
        end loop;
        report "Testbench finished";
        wait;
    end process;

end Behavioral;
