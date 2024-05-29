----------------------------------------------------------------------------------
-- Company: 
-- Engineer: 
-- 
-- Create Date: 11/06/2023 12:54:52 PM
-- Design Name: 
-- Module Name: REGn_testbench - Behavioral
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
use IEEE.NUMERIC_STD.ALL;

-- Uncomment the following library declaration if instantiating
-- any Xilinx leaf cells in this code.
--library UNISIM;
--use UNISIM.VComponents.all;

entity REGn_testbench is
        Generic (n : integer := 4);
end REGn_testbench;

architecture Behavioral of REGn_testbench is

    function to_stdlogic(i: integer) return std_logic is
    begin
        case i is
            when 0 => return '0';
            when 1 => return '1';
            when others => return 'X';
        end case;
    end function;

    Component REGn is 
        Generic (n : integer := n);
        Port (
            EN : in std_logic;
            Din : in std_logic_vector(n-1 downto 0);
            Dout : out std_logic_vector(n-1 downto 0)
        );
    end component;
    
    Component REGn_s is 
        Generic (n : integer := n);
        Port (
            EN : in std_logic;
            Din : in std_logic_vector(n-1 downto 0);
            Dout : out std_logic_vector(n-1 downto 0)
        );
    end component;
    
    signal e : std_logic;
    signal din, sout, bout : std_logic_vector(n-1 downto 0);
    
begin

    b: REGn port map(EN=>e, Din=>din, Dout=>bout);
    s: REGn_s port map(EN=>e, Din=>din, Dout=>sout);
    
    main: process
    begin 
        for i in 0 to n-1 loop 
            for j in 0 to 1 loop 
                e <= to_stdlogic(j);
                din <= std_logic_vector(to_unsigned(i, din'length));
               
                wait for 10 ns;
                
                assert (bout = sout)
                    report "testbench failed at num" & integer'image(i);
            end loop;
        end loop;
        wait;
    end process;

end Behavioral;
